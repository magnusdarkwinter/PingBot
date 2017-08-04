"use strict";

/***** Full height function start *****/
var setHeightWidth = function () {
    var height = $(window).height();
    var width = $(window).width();
    $('.full-height').css('height', (height));
    $('.page-wrapper').css('min-height', (height));

    /*Right Sidebar Scroll Start*/
    if (width <= 1007) {
        $('#chat_list_scroll').css('height', (height - 270));
        $('.fixed-sidebar-right .chat-content').css('height', (height - 279));
        $('.fixed-sidebar-right .set-height-wrap').css('height', (height - 219));

    }
    else {
        $('#chat_list_scroll').css('height', (height - 204));
        $('.fixed-sidebar-right .chat-content').css('height', (height - 213));
        $('.fixed-sidebar-right .set-height-wrap').css('height', (height - 153));
    }
    /*Right Sidebar Scroll End*/

    /*Vertical Tab Height Cal Start*/
    var verticalTab = $(".vertical-tab");
    if (verticalTab.length > 0) {
        for (var i = 0; i < verticalTab.length; i++) {
            var $this = $(verticalTab[i]);
            $this.find('ul.nav').css(
                'min-height', ''
            );
            $this.find('.tab-content').css(
                'min-height', ''
            );
            height = $this.find('ul.ver-nav-tab').height();
            $this.find('ul.nav').css(
                'min-height', height + 40
            );
            $this.find('.tab-content').css(
                'min-height', height + 40
            );
        }
    }
    /*Vertical Tab Height Cal End*/
};
/***** Full height function end *****/

/***** Resize function start *****/
$(window).on("resize", function () {
    setHeightWidth();
}).resize();
/***** Resize function end *****/

