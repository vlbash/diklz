this.buildContentTree = function (template, items, el, input, form, btn, status, changedItems) {
  self.createTreeData(template, el);
  self.setCheckedData(items, el);
  if (typeof status === "boolean") {
    
    self.setChangedItems(changedItems, el, status);
    self.editTreeContentListeners(el, status);
  }
  if (form) {
    var url = $(form).attr('action');
    self.submitTreeContent(btn, input, form, url, status);
  }
}

this.createTreeData = function (data, container) {
  var a = JSON.parse(data);

  $(container).html(self.createTreeDataTemplate(a.FirstLevels))
  self.addEventListenerContentTree(container);
}

this.createTreeDataTemplate = function (data) {
  var currDataObj = data,
    currEl;

  var html = '<ul class = "parent-item">';

  for (var k = 0; k < currDataObj.length; k++) {
    currEl = currDataObj[k];
    if (currEl.ChildItems && currEl.ChildItems.length) {
      html += '<li class="child-item"><div class="header-item">' +
        '<input id = "' +
        currEl.Code +
        '" class="checkbox header-checkbox" type="checkbox" name="' +
        currEl.Name +
        '"' +
        '/>' +
        '<label for="' +
        currEl.Code +
        '">' + currEl.Code + ' ' +
        currEl.Name;
        if (currEl.Info != null)
        {
          html += '<i class="icon-md icon icon-msg-info info-tree" title="' + currEl.Info + '"></i>';
        }
        html += '</label><button type="button" class="btn btn-square btn-tiny icon-sm btn-parent"></button></div>';
      html += self.createTreeDataTemplate(currEl.ChildItems);
    } else if (currEl.Caption !== null && currEl.Caption !== undefined) {
      html += '<li class="child-item last-item">' +
        '<input id = "' +
        currEl.Code +
        '" class="checkbox caption-checkbox" type="checkbox" name="' +
        currEl.Name +
        '"' +
        '/>' +
        '<label for="' +
        currEl.Code +
        '">' + currEl.Code + ' ' +
        currEl.Name;
        if (currEl.Info != null)
        {
          html += '<i class="icon-md icon icon-msg-info info-tree" title="' + currEl.Info + '"></i>';
        }
        html += '</label><input class="caption-input" type="text" disabled>';
    } else {
      html += '<li class="child-item last-item">' +
        '<input id = "' +
        currEl.Code +
        '" class="checkbox" type="checkbox" name="' +
        currEl.Name +
        '"' +
        '/>' +
        '<label for="' +
        currEl.Code +
        '">' + currEl.Code + ' ' +
        currEl.Name;
        if (currEl.Info != null)
        {
          html += '<i class="icon-md icon icon-msg-info info-tree" title="' + currEl.Info + '"></i>';
        }
        html += '</label>';
    }
    html += '</li>'
  }
  html += '</ul>'
  return html
}

// this.addEventListenerContentTree = function (el) {
//   var parentBtn,
//     parentHeader,
//     parentCheckbox,
//     childItem,
//     childCheckbox,
//     inputCaption;

//   parentBtn = $(el).find('.btn-parent');
//   parentHeader = $(el).find('.header-item');
//   parentCheckbox = $(parentHeader).find('input[type="checkbox"]');
//   childItem = $(el).find('.child-item');
//   childCheckbox = $(childItem).find('input[type="checkbox"]');
//   inputCaption = $(childCheckbox).siblings('input.caption-input');
//   $(el).on("change", "input[type='checkbox']", function () {
//     var headerItem = $(this).closest('.header-item');
//     var childInputs = headerItem.siblings(".parent-item").find("input[type='checkbox']").not('.caption-checkbox, .checkbox-disabled');
//     if ($(this).is(':checked')) {
//       childInputs.prop('checked', true);
//     } else {
//       childInputs.prop('checked', false);
//     }
//   });

//   if ($(childCheckbox).parent().is('.caption-item')) {

//     $(childCheckbox).on('change', function () {
//       var currInputCaption,
//         currDisabledInput;
//       currInputCaption = $(this).siblings('input.caption-input');
//       currDisabledInput = currInputCaption.is('[disabled]');

//       if (currDisabledInput) {
//         $(currInputCaption).removeAttr('disabled');
//       } else {
//         $(currInputCaption).attr('disabled', 'disabled');
//       }
//       if ($(this).is(':not(:checked)')) {
//         var currParentHeader = $(this).parents('.parent-item').prev('.header-item');
//         $(currParentHeader).find("input[type='checkbox']").prop('checked', false);
//       }
//     });

//     $(inputCaption).on('change', function () {
//       var inputCaptionVal = $(this).val();
//       var InputcaptionCheckbox = $(this).siblings('input[type="checkbox"]');
//       $(InputcaptionCheckbox).attr('value', inputCaptionVal);
//     });
//   }

//   $(parentBtn).on('click', function () {
//     $(this).toggleClass('is-transform');
//     $(this).parent(parentCheckbox).next().slideToggle();
//   })
// }
this.addEventListenerContentTree = function (el) {
  var parentBtn,
      parentHeader,
      parentCheckbox,
      childItem,
      childCheckbox,
      inputCaption;
  parentBtn = $(el).find('.btn-parent');
  parentHeader = $(el).find('.header-item');
  parentCheckbox = $(parentHeader).find('input[type="checkbox"]');
  childItem = $(el).find('.child-item');
  childCheckbox = $(childItem).find('input[type="checkbox"]');
  inputCaption = $(childCheckbox).siblings('input.caption-input');
  $(el).on("change", "input[type='checkbox']", function () {
      var headerItem = $(this).closest('.header-item');
      var childInputs = headerItem.siblings(".parent-item").find("input[type='checkbox']");
      // if ($(this).is(':checked')) {
      // //     childInputs.prop('checked', true);
      // } else {
        if ($(this).is(':not(:checked)')) {
          childInputs.prop('checked', false);
          var caption = childInputs.siblings('input.caption-input');
          for (var i = 0; i < caption.length; i++) {   
            $(caption[i]).attr('disabled', 'disabled');
        }
      }
      if ($(this).is(':not(:checked)')) {
          var currParentHeaderNotChecked = $(this).parents('.parent-item').prev('.header-item');
          for (i = 0; i < currParentHeaderNotChecked.length; i++) {
              var childInputs = $(currParentHeaderNotChecked[i]).next(".parent-item")
                  .find("input[type='checkbox']");
              var checkInputs = false;
              for (var j = 0; j < childInputs.length; j++) {
                  if (childInputs[j].checked == true) {
                      checkInputs = true;
                  }
              }
              if (checkInputs != true)
                  $(currParentHeaderNotChecked[i]).find("input[type='checkbox']").prop('checked', false);
          }
      }
  });
  if ($(childCheckbox).parent().is('.last-item')) {
      $(childCheckbox).on('change', function () {
          var currInputCaption,
              currDisabledInput;
          currInputCaption = $(this).siblings('input.caption-input');
          currDisabledInput = currInputCaption.is('[disabled]');
          if (currDisabledInput) {
              $(currInputCaption).removeAttr('disabled');
          } else {
              $(currInputCaption).attr('disabled', 'disabled');
          }
          if ($(this).is(':checked')) {
              var currParentHeader = $(this).parents('.parent-item').prev('.header-item');
              $(currParentHeader).find("input[type='checkbox']").prop('checked', true);
          }
      });
      $(inputCaption).on('change', function () {
          var inputCaptionVal = $(this).val();
          var InputcaptionCheckbox = $(this).siblings('input[type="checkbox"]');
          $(InputcaptionCheckbox).attr('value', inputCaptionVal);
      });
  }
  $(parentBtn).on('click', function () {
      $(this).toggleClass('is-transform');
      $(this).parent(parentCheckbox).next().slideToggle();
  })
  $(".info-tree").on('click', function(e){
      e.preventDefault();
      mt.createDialog(null ,$(this).attr("title"));
  })
}

this.setCheckedData = function (data, container) {
  if (data !== null && data.length > 0) {
    var checkedItemsObj = JSON.parse(data);
    for (item in checkedItemsObj) {
      var currItem = checkedItemsObj[item]
      var currId = currItem.id;
      var currVal = currItem.value;
      var trimCurrId = currId.replace(/(?:\.)/g, '\\.')

      var findItem = $(container).find("#" + trimCurrId);
      $(findItem).prop('checked', true);
      if ($(findItem).hasClass('caption-checkbox')) {
        $(findItem).val(currVal);
        $(findItem).siblings('.caption-input').val(currVal).removeAttr('disabled');
      }
    }
  }
}

this.setChangedItems = function(data, container, status){
  if (data !== null && data.length > 0) {
    var checkedItemsObj = JSON.parse(data);
    for (item in checkedItemsObj) {
      var currItem = checkedItemsObj[item]
      var currId = currItem.id;
      var currVal = currItem.value;
      var trimCurrId = currId.replace(/(?:\.)/g, '\\.')

      var findItem = $(container).find("#" + trimCurrId);
      
      if (status){
        $(findItem).prop('checked', false);
        $(findItem).addClass('checkbox-deleted');
      } else{
        $(findItem).prop('checked', true);
        $(findItem).addClass('checkbox-added');
      }
      if ($(findItem).hasClass('caption-checkbox')) {
        $(findItem).val(currVal);
        $(findItem).siblings('.caption-input').val(currVal).removeAttr('disabled');
      }
    }
  }
}

this.getTreeContentData = function (form, input, status) {
  var resultContainer = $(form).find('#content-tree'),
    objJson = [];
  if (status === true) {
    checkedArr = $(resultContainer).find('.checkbox-deleted').not('.checkbox-disabled');
  } else if(status === false){
    checkedArr = $(resultContainer).find('.checkbox-added').not('.checkbox-disabled');
  } else {
    checkedArr = $(resultContainer).find('input:checked');
  }
  for (var k = 0; k < checkedArr.length; k++) {
    var currInput = checkedArr[k],
      currInputId = $(currInput).attr('id'),
      currInputVal = $(currInput).attr('value'),
      pushTo = {
        id: currInputId,
        value: currInputVal
      };
    objJson.push(pushTo);
  }
  var obj = JSON.stringify(objJson);
  $(form).find(input).val(obj);
}

this.submitTreeContent = function (btn, input, form, url, status) {
  $(btn).on('click', function (e) {
    e.preventDefault();
    self.getTreeContentData(form, input, status);
    
    if (self.validateForm(form, url)) {
      $.ajax({
          type: 'post',
          url: url,
          data: $(form).serialize(),
        })
        .done(function (result) {
          if (result.status === "success") {          
            goBackWithRefresh();           
            return false;
            function goBackWithRefresh() { // https://stackoverflow.com/questions/25639290/windows-history-back-location-reload-jquery
              if ('referrer' in document) {
                window.location = document.referrer + result.tab;
              } else {
                window.history.back(-1);
              }              
            }            
          } else {
            console.log('response_status', result.status)
          }
        })
    } else {
      $('html, body').animate({
        scrollTop: ($('.asp-validation.active').first().offset().top - 180)
      }, 700);
    }
  });
}

this.editTreeContentListeners = function (el, status) {
  var unCheckedInputs = $('#content-tree .checkbox:not(:checked)'),
    checkedInputs = $('#content-tree .checkbox:checked');

  if (status) {
    $(unCheckedInputs).not('.checkbox-deleted').addClass('checkbox-disabled');
    if($(unCheckedInputs).hasClass('caption-checkbox')){
      $(unCheckedInputs).siblings('.caption-input').attr('disabled', true);
    };
  } else {
    $(checkedInputs).not('.checkbox-added').addClass('checkbox-disabled');
    if($(checkedInputs).hasClass('caption-checkbox')){
      $(checkedInputs).not('.checkbox-added').siblings('.caption-input').attr('disabled', true);
    };
  }

  $(el).on('change', "input[type='checkbox']", function () {
    var headerItem = $(this).closest('.header-item'),
      childInputs = headerItem.siblings(".parent-item").find("input[type='checkbox']").not('.caption-checkbox, .checkbox-disabled'),
      currParentHeader = $(this).parents('.parent-item').prev('.header-item');

    if (status) {
      self.removeContentItemClass($(this), childInputs);
      if ($(this).is(':not(:checked)')) {
        $(currParentHeader).find("input[type='checkbox']").not('.checkbox-disabled').addClass('checkbox-deleted');
      }

    } else {
      self.addContentItemClass($(this), childInputs);
      if ($(this).is(':not(:checked)')) {
        $(currParentHeader).find("input[type='checkbox']").removeClass('checkbox-added');
      }
    }

  });
}

this.removeContentItemClass = function (el, items) {
  if ($(el).is(':checked')) {
    $(el).removeClass('checkbox-deleted');
    $(items).removeClass('checkbox-deleted');
  } else {
    $(el).addClass('checkbox-deleted');
    $(items).addClass('checkbox-deleted');
  }
}
this.addContentItemClass = function (el, items) {
  if ($(el).is(':checked')) {
    $(el).addClass('checkbox-added');
    $(items).addClass('checkbox-added');
  } else {
    $(el).removeClass('checkbox-added');
    $(items).removeClass('checkbox-added');
  }
}
 this.currentTab = function(){  
    var currentUrl = $(location).attr('href');
    var tabId = currentUrl.split('#')[1];
    $("#" + tabId).trigger("click");
}