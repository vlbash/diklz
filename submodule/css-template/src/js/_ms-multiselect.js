this.setLabelMultiSelect = function (el) {

    var ms = el,
        msWrap = $(ms).siblings('.ms-options-wrap'),
        msHiddenLabel = $(msWrap).find('button span.ms-hidden-label');

    $(ms).bind('change', function (e) {
        e.stopPropagation();
        var msEl = $(this),
            msSelected = $(msEl).siblings('.ms-options-wrap.ms-has-selections');

        setTimeout(function () {
            var labelArr = [],
                listItems = [],
                labelHtml,

                //find & get data from control button
                labelData = $(msHiddenLabel).html().split(', '),

                //find & get data from selected options
                selectedList = $(msWrap).find('.ms-options ul li.selected');

            for (var i = 0; selectedList.length > i; i++) {
                var t = $(selectedList[i]).text()
                listItems.push(t)
            }

            //check quantity of items & set labelArr
            if (listItems.length > labelData.length) {
                labelArr = labelData;
            } else if (listItems.length == labelData.length) {
                labelArr = listItems;
            } else {
                $(msSelected).find('button .ms-label').remove();
            }

            //clear html data after change
            labelHtml = '';
            for (var i = 0; labelArr.length > i; i++) {
                //clear control button after change
                $(msSelected).find('button .ms-label').remove();
                //prepare new html data
                labelHtml += '<div class="ms-label">' + labelArr[i] + '</div>';
            }

            //toggle control button visibility
            if ($(msWrap).hasClass('ms-has-selections')) {
                $(msHiddenLabel).addClass('hidden');
            } else {
                $(msHiddenLabel).removeClass('hidden');
            }

            self.addMslabel(msEl, labelHtml)
        }, 0);


    })
}

this.addMslabel = function (item, data) {
    //add ms-label
    var t = $(item).siblings('.ms-options-wrap.ms-has-selections').find('button');
    $(data).appendTo(t);
}

if ($('select').is('[multiple].multiselect')) {
    $('[multiple].multiselect').each(function () {
        var selectPlaceholder = $(this).attr('data-placeholder');
        $(this).multiselect({
            columns: 1,
            selectAll: true,
            texts: {
                placeholder: selectPlaceholder,
                selectAll: 'Обрати все',
                unselectAll: 'Видалити всі позначки'
            }
        });
        self.setLabelMultiSelect($(this));
    })
}

if ($('select').is('[multiple].multiselect-search')) {
    $('[multiple].multiselect-search').each(function () {
        var selectPlaceholder = $(this).attr('data-placeholder');
        $(this).multiselect({
            columns: 1,
            search: true,
            selectAll: true,
            texts: {
                placeholder: selectPlaceholder,
                search: 'Пошук',
                selectAll: 'Обрати все',
                unselectAll: 'Видалити всі позначки'
            }
        });
        self.setLabelMultiSelect($(this));
    })
}

if ($('.ms-options-wrap .ms-options')) {
    setTimeout(function () {
        $('.ms-options-wrap .ms-options').each(function () {
            self.createSelectScrollBar($(this).get(0));
        })
    }, 0)
}