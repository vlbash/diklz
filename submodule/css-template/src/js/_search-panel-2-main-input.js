
this.createMainText = function (formObj) {//filterForm !!!!!!!!!!!

     var text = formObj.mainInputText || 'Для формування звіту виберіть параметри';
     $(formObj.mainText).text(text);
};

this.createMainInput = function (formObj) {//searchForm

     var mainInput = formObj.mainInput,
          mainInputData = formObj.mainInputConfig;

     if (mainInputData.labelName) {
          self.addAttr(mainInput, 'placeholder', mainInputData.labelName);
     } else {
          self.addAttr(mainInput, 'placeholder', 'Пошук');
     }
     if (mainInputData.type) {
          self.addAttr(mainInput, 'type', mainInputData.type);
     } else {
          self.addAttr(mainInput, 'type', 'text');
     }

     mainInput.val('');

     self.manageMainInput(formObj);
};


this.manageMainInput = function (formObj) {

     $(formObj.mainInput).keydown(function (e) {
          if (e.keyCode === 13) {
               e.preventDefault();

               if (formObj.filled) {
                    self.clearSearchForm(formObj, true);
               }

               self.submitMainInput(formObj);
          }
     });

     $(formObj.formWrapper).find('.content-search-submit-main-input').each(function () {
          $(this).on('click', function () {
               if (formObj.filled) {
                    self.clearSearchForm(formObj, true);
               }
               self.submitMainInput(formObj);
          });
     });

};

this.submitMainInput = function (formObj) {

     if (formObj.controlConfig){
          var query = self.prepareSearchQueryData([{
               'name': formObj.mainInputConfig.mainInputLookUp[0],
               'value': $(formObj.mainInput).val()
          },
          {              
               'name': formObj.controlConfig.appState[0],
               'value': formObj.controlConfig.value                 
          }]);
     }else{
          var query = self.prepareSearchQueryData([{
               'name': formObj.mainInputConfig.mainInputLookUp[0],
               'value': $(formObj.mainInput).val()
          }]);
     }
     
     if (!self.isElValueEmpty(formObj.mainInput)) {
          formObj.shownFilters = [{
               'name': formObj.mainInputConfig.mainInputLookUp[0],
               'shownName':formObj.mainInputConfig.labelName,
               'value': $(formObj.mainInput).val(),
               'shownValue': $(formObj.mainInput).val()
          }];

     } else {
          formObj.shownFilters = [];
     }

     self.ajaxSubmitSearchForm(formObj, query);

};

this.clearMainInput = function (formObj) {

     $(formObj.mainInput).val('');
}
