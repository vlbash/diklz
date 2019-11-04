this.openModal = function () {
   $(modal).addClass('open');
   $(modalCloseBtn).on('click', function () {
      self.closeModal();
   });
}

this.closeModal = function () {
   $(modal).removeClass('open');
   self.clearElEventListeners($(modalContainer));
   setTimeout(function () {
      self.toggleModalClass(modal);
      self.clearModal();
   }, 300)
}

this.clearModal = function () {
   $(modalContainer).html('');
   $(modalContainer).removeAttr('data-tab-container-url');
}

this.getModalContent = function (url) {
   $(modalContainer).attr('data-tab-container-url', url);
   //console.log(self.tryLoadContainer($(modal).find('[data-tab-container-url]')))
   self.tryLoadContainer($(modal).find('[data-tab-container-url]'));

}

this.getModalPosition = function (el) {
   var pos = $(el).attr('data-modal');
   switch (pos) {
      case 'modal-right':
         self.toggleModalClass(modal, pos)
         break;
      case 'modal-left':
         self.toggleModalClass(modal, pos)
         break;
      case 'modal-center':
         self.toggleModalClass(modal, pos)
         break;
      case 'modal-top':
         self.toggleModalClass(modal, pos)
         break;
      case 'modal-bottom':
         self.toggleModalClass(modal, pos)
         break;
      case 'modal-fs':
         self.toggleModalClass(modal, pos)
         break;
      default:
         self.toggleModalClass(modal, 'modal-center')
   }
}

this.toggleModalClass = function (el, name) {
   if ($(el).is('[class^="modal-"]')) {
      $(el).removeClass(function (index, className) {
         return (className.match(/(^|\s)modal-\S+/g) || []).join(' '); //https://stackoverflow.com/questions/2644299/jquery-removeclass-wildcard
      });
   } else {
      $(el).addClass(name);
   }
}

//activate modal btn
if (modalOpenBtn) {
   $(modalOpenBtn).on('click', function () {
      var dataUrl = $(this).attr('data-tab-container-url');
      self.getModalPosition($(this));
      $.when(self.getModalContent(dataUrl))
         .done(self.closeAllOpenLi(headerAccountMenu))
         .done(function () {
            setTimeout(function () {
               self.openModal();
            }, 300)
         })
   })
}

//add eventListenet for modal btn
this.activateModalBtn = function (el) {
   $(el).find('[data-modal]').each(function () {
      $(this).on('click', function () {
         var dataUrl = $(this).attr('data-tab-container-url');
         self.getModalPosition($(this));
         $.when(self.getModalContent(dataUrl))
            .done(self.closeAllOpenLi(headerAccountMenu))
            .done(function () {
               setTimeout(function () {
                  self.openModal();
               }, 300)
            })
      })
   })
}