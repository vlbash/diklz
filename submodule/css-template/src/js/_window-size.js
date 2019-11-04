
/**
 * размер страницы с учетом прокрутки
 */

this.returnScrollHeight = function () {

    return Math.max(
        document.body.scrollHeight, document.documentElement.scrollHeight,
        document.body.offsetHeight, document.documentElement.offsetHeight,
        document.body.clientHeight, document.documentElement.clientHeight
    );
}


/**
 * высота видимой области окна
 */
this.returnWindowHeight = function(){
    return window.innerHeight;
}

/**
 * // вся ширина окна
 * 
 */

this.returnWindowInnerWidth = function(){
    return window.innerWidth;
}
/**
 * // ширина минус прокрутка
 */
this.returnClientWidth = function(){
    document.documentElement.clientWidth
}