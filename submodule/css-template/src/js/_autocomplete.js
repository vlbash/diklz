this.autocompleteEl = function (el) {
   var url = $(el).attr("autocomplete-url"); //url to contoller where we can receive array with properties
   var idinputname = $(el).attr("autocomplete-idinput-name"); //set name & id for hidden input near lookup
   var initid = $(el).attr("autocomplete-init-id"); //set value for hidden input when we have saved data
   var onsuccess = $(el).attr("autocomplete-onsuccess"); // testing => ?? why
   var onselect = $(el).attr("autocomplete-onselect"); //call function onchange => select event autocomplete jquery-ui
   var onblur = $(el).attr("autocomplete-onblur"); //testing => after focus ?? why
   var paramsstr = $(el).attr("autocomplete-params"); //testing => ?? why
   var allowcreate = $(el).attr("autocomplete-allowcreate") === ""; //testing => add this block and after next focus value will saved ?? why
   params = paramsstr ? paramsstr.split(",") : null;
   var form = $(el).closest("form");

   //get name from lookup input
   var name = idinputname ? idinputname : $(el).attr("name").replace("Name", 'Id');
   if (!name.endsWith('Id')) {
      name += 'Id';
   }

   //find hidden input near lookup
   var hidden = $(form).find("input[name='" + name + "']");
   if ($(hidden).length === 0) {

      //set attr name & id for hidden input
      $(el).after('<input type="hidden" name="' + name + '" id="' + name + '"/>');
      hidden = $(form).find("input[name='" + name + "']");

      //set value for hidden input
      hidden.val(initid ? initid : '');
   }

   // if datePicker set datepicker value
   //
   // if checkbox set

   //autocomplete - function from jquery ui
   $(el).autocomplete({
      source: function (request, response) {
         // A request object, with a single term property, which refers to the value currently in the text input. 
         var data = {};
         if (params) {
            console.log(params);
            $.each(params, function (i, p) {
               data[p] = $('#' + p).val();

            });
         }
         data['term'] = request.term;

         $.ajax({
            url: url,
            type: "POST",
            dataType: "json",
            data: data,
            success: function (data) {
               //receive array with properties
               if (onsuccess) {
                  new Function('data', onsuccess)(data);
                  console.log(data) ///?????? onsuccess
               }
               if (data.length === 0 && !allowcreate) {
                  data.push({
                     label: 'Відповідності не знайдено',
                     value: '',
                     option: ''
                  });
               }
               if (!allowcreate) {
                  hidden.val('');
               }
               for (var k = 0; k < data.length; k++) {
                  item = data[k];
                  if (item.label == request.term) {
                     //set value for hidden option
                     hidden.val(item.option);
                     break;
                  }
               };
               response(
						// method applies a function to each item in an array or object and maps the results into a new array ?? why
                  $.map(data, function (item) {
                     return {
                        label: item.label,
                        value: item.value,
                        option: item.option,
                        data: item.data
                     };
                  }));
            }
         });
      },
      classes: {
         "ui-autocomplete": "input-group-autocomplete"
      },
      minLength: 0,
      select: function (event, ui) {
         if (ui.item.option === '') {
            ui.item.value = '';
         }
         hidden.val(ui.item ? ui.item.option : '');
         if (onselect) {
            new Function('item', onselect)(ui.item);
         }
      },
      create: function (e) {
         $('.ui-helper-hidden-accessible').remove();
      }
   }).focus(function () {
      var term = $(this).val();
      $(this).data('uiAutocomplete').search(term ? term : '');
   });

   // $(el).on('blur', function () {
   //     if (hidden) {
   //         //if hidden input
   //         if (!$(el).val()) {
   //             $(hidden).val('');

   //         } else {
   //             var v = $(hidden).val();
   //             if (v === "" && !allowcreate) {
   //                 this.value = '';
   //             }
   //         }

   //         if (onblur) {
   //             new Function('value', onblur)($(hidden).val());
   //         }
   //     }
   // });

}


if ($('*').is('[autocomplete-url]')) {
   $('[autocomplete-url]').each(function () {
      self.autocompleteEl($(this));
   })
}