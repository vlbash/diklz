// body.niceScroll({
//      cursorcolor: "#e0e0e0",
//      cursorwidth: "8px"
// });
this.updateNiceScroll = function () {
     body.getNiceScroll().resize();
}


this.createSelectScrollBar = function (el) { // может быть только один на странице в одно и тоже время - когда один элемент открываетсяы, иной закрывается, следить за вложенными элементыыми
     selectScrollBar = new PerfectScrollbar(el, {
          wheelSpeed: 0.5,
          minScrollbarLength: 20,
          maxScrollbarLength: 60
     });
};
this.destroySelectScrollBar = function () {
     if (selectScrollBar) {
          selectScrollBar.destroy();
          selectScrollBar = null;
     }
};

this.scrollTop = function () {
     var scrollTopButton = $('#scroll-top');

     //window scroll event
     $(window).scroll(function () {
          if ($(window).scrollTop() > windowHeight/4) {
               $(scrollTopButton).removeClass('hidden');
          } else {
               $(scrollTopButton).addClass('hidden');
          }
     })

     //button-scrollTop listener
     $(scrollTopButton).on('click', function(){
          $('html, body').animate({scrollTop : 0},600);
          $(this).addClass('hidden');
     })
}