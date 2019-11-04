moment.locale('uk');


this.sortArrObjByMethod = function (arr, method) {

    function sortByMethod(arr1, arr2) {
        var a = arr1[method],
            b = arr2[method],
            temp;

        a = +a;
        b = +b;
        if (a != a || b != b) {//проверка на NaN, если NaN - сортируй, как string, иначе - как number
            temp = arr1[method] - arr2[method];
        } else {
            temp = a - b;
        }
        return temp;
    }
    var tempArr = arr.sort(sortByMethod);

    return tempArr;
}

this.addAttr = function (el, attrName, attrVal) {
    el.attr(attrName, attrVal);
}
this.addData = function (el, dataName, dataVal) {
    el.data(dataName, dataVal);
}

this.calculateWindowHeight = function() {
    return $(window).height();
}
this.calculateWindowWidth = function() {
    return $(window).width();
}



this.clearElEventListeners = function (el) {
    $(el).off();
    $(el).find('*').each(function () {
        $(this).off();
    });
}

this.generateId = function(){
    return '_' + Math.random().toString(36).substr(2, 9);
}

this.isElValueEmpty = function (el) {
    if($.trim($(el).val()) == ''){
        return true;
    }
    return false;
}

this.isStringEmpty = function name(str) {
    if($.trim(str) == ''){
        return true;
    }
    return false;
}

this.returnTagName = function (el) {
    return  $(el).prop('tagName').toLowerCase();
}