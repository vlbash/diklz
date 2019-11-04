this.createMessenger = function () {
    var frameWrapper = $('#frame-wrapper'),
        widgetHidden = true,
        closeButton = frameWrapper.find('.frame-close'),
        win = $(window),
        winHeight = 0,
        winWidth = 0,
        resizeTimer,
        frame = '<iframe class = "frame" src="' + frameWrapper.data('url') + '"  frameborder="0" scrolling="no"></iframe>',
        wrapperWidth = parseInt(frameWrapper.css('width')),
        wrapperHeight = parseInt(frameWrapper.css('height'));


    $('#show-messenger').on('click', function () {
        if (!widgetHidden)
            return;
        $.when(createFrame()).then(showFrame()).then(self.closeAllOpenLi($('#header-account-menu')));
    })

    $(closeButton).on('click', function () {
        hideFrame();
        setTimeout(function () {
            destroyFrame()
        }, timeOutInterval);
    })


    function showFrame() {
        startFunction();
        frameWrapper.css({
            'z-index': '2000',
            'opacity': '',
            'max-width': '80%',
            'transition': '.05s',
            'top': '50%',
            'transform': 'translateY(-' + frameWrapper.height() / 2 + 'px)'
        }).animate({
            right: 0
        }, timeOutInterval);

    }

    function hideFrame() {
        widgetHidden = true;
        frameWrapper.animate({
            opacity: 0
        }, 500, function () {
            frameWrapper.css({
                'top': '',
                'right': '-390px',
                'max-width': '0',
                'left': '',
                'z-index': '-1',
                'transition': ''
            })
        })

    }

    function createFrame() {
        widgetHidden = false;
        frameWrapper.append(frame);
    }

    function destroyFrame() {
        $('.frame').remove();

    }

    function startFunction() {
        if (widgetHidden)
            return;
        setWinSize();
        setFrameSize();
    }

    function setWinSize() {
        winHeight = win.height();
        winWidth = win.width();
    };

    function setFrameSize() {
        if (winHeight < wrapperHeight) {
            frameWrapper.css({
                'height': winHeight + 'px',
                'transform': 'translateY(-' + winHeight / 2 + 'px)'
            });
        } else if (wrapperHeight != frameWrapper.height()) {
            frameWrapper.css({
                'top': '50%',
                'height': '',
                'transform': 'translateY(-' + frameWrapper.height() / 2 + 'px)'
            });

        }
        if (winWidth < wrapperWidth) {
            frameWrapper.css({
                'width': winWidth + 'px',
                'left': '0'
            });
        } else if (frameWrapper.width() < wrapperWidth) {
            frameWrapper.css({
                'width': wrapperWidth + 'px'
            });
        }

    };

    function dragTopLimit(top) {
        if (top < 0) {
            top = 0;
        }
        if (top > (winHeight - frameWrapper.height())) {
            top = winHeight - frameWrapper.height();
        }
        return top + frameWrapper.height() / 2
    };

    function dragLeftLimit(left) {
        if (left < 0) {
            left = 0;
        }
        if (left > (winWidth - frameWrapper.width())) {
            left = winWidth - frameWrapper.width();
        }
        return left;
    };
    frameWrapper.draggable({
        drag: function (event, ui) {
            ui.position.top = dragTopLimit(ui.position.top);
            ui.position.left = dragLeftLimit(ui.position.left);
        }
    });
    win.on('resize', function (e) {
        if (widgetHidden)
            return;
        clearTimeout(resizeTimer);
        resizeTimer = setTimeout(function () {
            startFunction();
        }, 62);
    });


}



if ($('span').is('#show-messenger')) {
    self.createMessenger()
}