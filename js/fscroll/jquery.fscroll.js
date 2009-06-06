(function($){
  $.fn.fscroll = function(options){
    var screenElem = $('<div id="__dummy"></div>')
    .css('position', 'absolute')
    .css('z-index', -9999)
    .css('bottom', 0)
    .css('width', '100%')
    .css('height', '100%')
    .css('right', 0).appendTo($('body'));

    var opts = $.extend({}, $.fn.fscroll.defaults, options);

    var scroll = function(me, origin){
      var scrollParent = me.offsetParent();
      var isBody = scrollParent.attr('nodeName') == 'BODY';
      var offestParent = isBody ? screenElem : scrollParent;

      var coords = {
        me: {
          x: me.position().left + (me.width()  / 2) - (isBody ? scrollParent.scrollLeft() : 0),
          y: me.position().top  + (me.height() / 2) - (isBody ? scrollParent.scrollTop()  : 0)
        },

        parent : {
          height: offestParent.height(),
          width : offestParent.width(),
          top : scrollParent.scrollTop(),
          left : scrollParent.scrollLeft()
        }
      };

      var scrollTop = coords.me.y + (coords.parent.top  - coords.parent.height / 2);
      var scrollLeft = coords.me.x + (coords.parent.left - coords.parent.width  / 2);

      if(Math.abs(scrollTop - coords.parent.top) > 5 || Math.abs(scrollLeft - coords.parent.left) > 5) {
        setTimeout(function(){
          if(origin == $.fn.fscroll.current){
            scrollParent.stop();

            var anim_params = {};

            if(opts.horizontal){
              anim_params.scrollLeft = scrollLeft;
            }

            if(opts.vertical){
              anim_params.scrollTop = scrollTop;
            }

            scrollParent.animate(anim_params, opts.scrollTime);
          }
        },
        opts.delay);

        if(!isBody){
          scroll(scrollParent, origin);
        }
      }
    }

    return this.bind('focus.fscroll', function(e){
        var me = $(this);
        $.fn.fscroll.current = me;
        scroll(me, me);
      });
  }

  $.fn.fscrollOff = function(){
    return this.unbind('focus.fscroll');
  }

  $.fn.fscroll.defaults = {
    scrollTime: 1000,
    delay: 500,
    vertical: true,
    horizontal: false
  };
})(jQuery);