this.setDataTabGroup = function (el) {

     if ($(el).find('[ data-tab-open]').length) {
          $(el).find('[ data-tab-open]').on('click', function () {
               self.showDataTabGroup(el)
          })
     }


     if($(el).find('select').length){
          $(el).find('select').each(function(){
               $(this).addClass('.select');
               self.selectWrap($(this));
          })
     }


     if($(el).find('input').length){
          $(el).find('input').each(function(){
               $(this).addClass('input');
               self.initInputAfterLoad($(this));
          });
     }
}

this.showDataTabGroup = function (el) {
     dataTabOpen = true;
     self.addOpenClass(el);
}



this.hideDataTabGroup = function () {
     $('.data-tab-group').each(function(){
          self.removeOpenClass($(this));
     })
     dataTabOpen = false;


}


this.manageOpenTab = function () {
     self.deleteRightBtnMenuAdditionalBtn();
     if ($(openTab).find('[data-tab]').length) {
          $(openTab).find('[data-tab]').each(function () {
               self.addRightBtnMenuAdditionalBtn($(this));
          });
     }

}

this.deleteRightBtnMenuAdditionalBtn = function () {
     if ($(rightBtnMenu).find('[data-tab]').length) {
          $(rightBtnMenu).find('[data-tab]').each(function () {
               self.clearElEventListeners($(this));
               $(this).off();
               $(this).detach();
          })
     }
     if ($(rightBtnMenu).find('[data-tab-main]').length) {
          $(rightBtnMenu).find('[data-tab-main]').each(function () {
               if ($(openTab).index()) {
                    $(this).css('display', 'none');
               } else {
                    $(this).css('display', '');
               }
          })
     }
     self.manageRightBtnMenu();
}

this.addRightBtnMenuAdditionalBtn = function (el) {
     var cloneEl = $(el).clone();
     //console.log($(el));
     // cloneElTagName = $(cloneEl).prop("tagName").toLowerCase();
     $(cloneEl).appendTo(rightBtnMenu);


     if ($(cloneEl).hasClass('data-tab-group')) {
          self.setDataTabGroup(cloneEl);
          self.setToolTipsForChild(cloneEl);
     } else {
          self.tooltipEl(cloneEl);
     }

     self.manageRightBtnMenu();
}