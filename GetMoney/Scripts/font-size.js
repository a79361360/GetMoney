// JavaScript Document
;
    (function (doc, win) {
        var max = 768,
        docEl = doc.documentElement,
        resizeEvt = 'orientationchange' in window ? 'orientationchange' : 'resize',
        recalc = function () {
            var clientWidth = docEl.clientWidth;
            if (!clientWidth) return;
            if (clientWidth >= max) {
                docEl.style.fontSize = '100px';
            } else {
                docEl.style.fontSize = 100 * clientWidth / max + 'px';
            }
        };
        if (!doc.addEventListener) return;
        win.addEventListener(resizeEvt, recalc, false);
        doc.addEventListener('DOMContentLoaded', recalc, false);
    })(document, window);