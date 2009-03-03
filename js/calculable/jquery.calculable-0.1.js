/*
 * @name Calculable
 * @desc a jQuery plugin to make textboxes support simple calculations (client-side only)
 *
 * @author sandesh247
 * @version 0.1  (03/03/2009)
 *  
 * @type jQuery
 * @requires jQuery v1.3.1+ (not tested on versions prior to 1.2.6)
 *
 * Dual licensed under the MIT and GPL licenses:
 * http://www.opensource.org/licenses/mit-license.php
 * http://www.gnu.org/licenses/gpl.html
 *
 */
 
; (function($) {
    $.fn.calculable = function() {
        return this.each(function() {
            $(this).focus(function(e) {
                var expr = $(this).attr('calcExpr');
                if (expr) {
                    $(this).val(expr);
                }
            }); // end focus

            $(this).blur(function(e) {
                try {
                    var expr = $(this).val();
                    var calculated = eval('with(Math){' + expr + '}');

                    $(this).attr('calcExpr', expr);
                    $(this).val(calculated);
                }
                catch (e) {
                    $(this).attr('calcExpr', '');
                }
            }); // end blur
        }); // end each
    };
})(jQuery);