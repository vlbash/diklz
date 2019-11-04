this.fastSelectControl = function (el) {

    el = $(el)
    el.fastselect({
        placeholder: '',
    searchPlaceholder: '',
    noResultsText: 'Результат не знайдено',
        
    });
    if (el.attr("readonly") !== undefined) {
        el.prev(".fstControls").find("button, input").remove();
    };
    el.on('change', function () {
        console.log('onchange', el.val())
    })

    el.on('focus',function(){
        console.log('focus', el.val())
    })
    el.on('blur',function(){
        console.log('blur', el.val())
    })


}

if ($('select').is('.fastselect')) {
    $('.fastselect').each(function () {
        self.fastSelectControl($(this));
    })
}