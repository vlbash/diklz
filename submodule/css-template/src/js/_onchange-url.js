this.onChangeUrl = function (onChangeEl, onchangeUrl) {
   var url;
   var args = {};
   if (onchangeUrl) {
      url = onchangeUrl;
      args[$(onChangeEl).attr('name')] = $(onChangeEl).val();
   } else {
      url = $(onChangeEl).attr("onchange-url");
      $("[onchange-url='" + url + "']").each(function () {
         args[$(this).attr('name')] = $(this).val();
      });
   }
   var changedInput = body.find('[onchange-url]');
   if ($(changedInput).val().length > 0) {
      $.ajax({
         url: url,
         type: "POST",
         data: args,
         success: function (data) {
            response = $.parseJSON(JSON.stringify(data));
            $.each(Object.keys(response), function (index, key) {
               var tVal = response[key];
               //console.log(tVal);
               if (tVal) {
                  var elements;
                  if (key.match('^#')) {
                     elements = $(key);
                  } else {
                     elements = $("[name='" + key + "']");
                  }

                  $.each(elements, function (index, element) {
                     var tTag = self.returnTagName(element);

                     if (tTag === 'select') {

                        var opt;
                        var options = $(elements).find('option');
                        var parentSelect = $(elements).closest('.select-group-inner');
                        var parentReadOnly = $(parentSelect).hasClass('select-readonly');
                        if (parentReadOnly) {
                           $(element).html('');
                           for (var i = 0; i < tVal.length; i++) {
                              opt = document.createElement('option');
                              opt.value = tVal[i].value;
                              opt.innerHTML = tVal[i].text;
                              if (tVal[i].selected) {
                                 opt.setAttribute('selected', 'selected');
                              }
                              if (tVal[i].disabled) {
                                 opt.setAttribute('disabled', 'disabled');
                              }
                              $(element).append(opt);
                           }
                        } else {
                           for (var i = 0; i < options.length; i++) {
                              opt = options[i];

                              if (tVal == opt.value) {
                                 $(opt).prop('selected', 'selected');
                                 opt.setAttribute('selected', 'selected');
                              }
                           }
                        }

                        if (tVal.length <= 0) {
                           $(element).html('');
                        }

                        self.updateSelectList($(element));
                     } else if (tTag === 'input') {
                        $(element).val(tVal);
                        self.addEmptyNotEmptyClass($(element));
                     } else {
                        $(element).text(tVal);
                     }
                  });
               } else if(tVal == null){
                  // var elements1;
                  // if (key.match('^#')) {
                  //    elements1 = $(key);
                  // } else {
                  //    elements1 = $("[name='" + key + "']");
                  // }
                  // $.each(elements1, function (index, element1) {
                  //    console.log($(element1));
                  //    $(element1).val('');
                  //    $(element1).text('');
                  // });

                  
               }
            });
         }
      });
   }

}

$(document).on('change', '[onchange-url]', function (e) {
   self.onChangeUrl($(this));
});