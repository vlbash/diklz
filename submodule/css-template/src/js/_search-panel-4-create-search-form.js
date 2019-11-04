this.createFormTemplate = function (formObj) {

     var inputConfig = formObj.controls,
          currData,
          tempHtml,
          inputLength = inputConfig.length,
          html = '';

     for (var i = 0; i < inputLength; i++) {
          currData = inputConfig[i];
          // if (i == 0) {
          //      self.configureMainInput(formObj, currData);
          // }

          formObj.hasFields = true; // дойдет сюда только если есть какие-то поля, кроме главного;

          tempHtml = self.selectDataTemplate(currData, true);
          
          html += '<li class="content-search-form-inner">' + tempHtml + '</li>';
     }

     return html;
};
