this.toggleActiveClass = function (el) {
    if ($(el).hasClass('active')) {
        self.removeActiveClass(el);
    } else {
        self.addActiveClass(el);
    }
}

this.addNotActiveClass = function (el) {
    $(el).addClass('not-active');
}

this.removeNotActiveClass = function (el) {
    $(el).removeClass('not-active');
}
this.addActiveClass = function (el) {
    $(el).addClass('active');
}

this.addNotDisabledClass = function (el) {
    $(el).addClass('disabled');
}
this.removeNotDisabledClass = function (el) {
    $(el).removeClass('disabled');
}
this.addDisabledClass = function (el) {
    $(el).addClass('disabled');
}

this.removeActiveClass = function (el) {
    $(el).removeClass('active');
}

this.toggleOpenClass = function (el) {
    if ($(el).hasClass('open')) {
        self.removeOpenClass(el);
    } else {
        self.addOpenClass(el);
    }
}

this.addOpenClass = function (el) {
    $(el).addClass('open');
}

this.removeOpenClass = function (el) {
    $(el).removeClass('open');
}

this.addTestClass = function (el) {
    $(el).addClass('test');
}
this.removeTestClass = function (el) {
    $(el).removeClass('test');
}

this.listColumnSwitch = function (el) {
    $(el).on('click', function () {
        $(mainWrapper).toggleClass("wrapper-column wrapper-list");
        self.updateNiceScroll();
    })
}


this.addEmptyNotEmptyClass = function (el) {
    if (!el.is(':visible')) {
        return
    }

    if (self.isElValueEmpty(el)) {
        $(el).removeClass('not-empty');
        $(el).addClass('empty');

    } else {
        $(el).removeClass('empty');
        $(el).addClass('not-empty');

    }

    if ($(el).closest('.datepicker-group')) {
        self.addDatePickerGroupEmptyNotEmptyClass(el);
    }
}



this.addDatePickerGroupEmptyNotEmptyClass = function (el) {

    var parent = $(el).closest('.datepicker-group'),
        children = $(parent).find('.input ,.select'),
        notEmpty = false;

    $(children).each(function () {
        if (!$(this).is(':visible') && self.returnTagName == 'input') {
            return
        }
        if (!self.isElValueEmpty($(this))) {
            notEmpty = true
        }
    })

    if (notEmpty) {
        $(parent).removeClass('empty');
        $(parent).addClass('not-empty');

    } else {
        $(parent).removeClass('not-empty');
        $(parent).addClass('empty');
    }

}



if ($('*').is(".btn-column-list")) {
    $('.btn-column-list').each(function () {
        self.listColumnSwitch($(this));
    })
}
if (mainWrapper) {
    
    $(mainWrapper).find('.btn-column-list').each(function(){
        if(mainWrapper.hasClass('wrapper-list')){
            $(this).removeClass('icon-list').addClass('icon-columns'); 
        }
        if(mainWrapper.hasClass('wrapper-column')){
            $(this).removeClass('icon-columns').addClass('icon-list');
        }
        $(this).on('click', function(){
            if(mainWrapper.hasClass('wrapper-list')){
                $(this).removeClass('icon-list').addClass('icon-columns'); 
            }
            if(mainWrapper.hasClass('wrapper-column')){
                $(this).removeClass('icon-columns').addClass('icon-list');
            }
        })
    });
}