this.initAllInputTextareaAfterLoad = function () {
    if ($('input').is('.input')) {
        $('.input').each(function () {
            self.initInputAfterLoad($(this));
        })
    }
    if ($('textarea').is('.textarea')) {
        $('.input').each(function () {
            self.initTextareaAfterLoad($(this));
        })
    }

}



this.initTextareaAfterLoad = function (textarea) {
    $('textarea').on('blur', function () {
        self.addEmptyNotEmptyClass($(this));
    })
    $('textarea').on('focus', function () {
        self.closeAllSelect(body);
    })
}

this.initInputAfterLoad = function (input) {

    $(input).on('blur', function () {
        self.addEmptyNotEmptyClass($(this));
    })

    $(input).on('focus', function () {
        var inputValue = $.trim($(this).val());
        if (inputValue == '0' || inputValue == '0.00' || inputValue == '0,00' || inputValue == '0,0' || inputValue == '0.0') {
            $(this).select();
        };
        self.closeAllSelect(body);
    })
    self.addEmptyNotEmptyClass(input);

}




this.onLoadCheckAllInputs = function () {
    $('input').each(function () {
        self.addEmptyNotEmptyClass($(this));
        if ($(this).is('[readonly]')) {
            $(this).addClass('input-readonly')
        }
        if ($(this).hasClass('input-file')) {
            self.inputFileCustomization($(this));
        }
    })
    $('textarea').each(function () {
        self.addEmptyNotEmptyClass($(this));
    })
}



this.inputFileCustomization = function (input) {
    var label = $(input).next('label'),
        labelVal = $(label).val();
    $(input).on('change', function (e) {
        var fileName = '';

        if (this.files && this.files.length > 1) {
            fileName = (this.getAttribute('data-multiple-caption') || '').replace('{count}', this.files.length);
        } else if (e.target.value) {
            fileName = e.target.value.split('\\').pop();
        }

        if (fileName) {
            $('.content-upload-filename').html(fileName);
        } else {
            //$(label).html(labelVal);
        }
    });
}