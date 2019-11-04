var userDisplaySettings = {
  userDisplaySettingsConfig: [
    {
      labelName: "Оберіть ширину екрану",
      name: "windowWidth",
      type: "select",
      selectOptions: [
        {
          Disabled: false,
          Group: null,
          Selected: false,
          Text: "На всю ширину",
          Value: 0
        },
        {
          Disabled: false,
          Group: null,
          Selected: true,
          Text: "Широка таблиця",
          Value: 1
        },
        {
          Disabled: false,
          Group: null,
          Selected: false,
          Text: "Вузька таблиця",
          Value: 2
        }
      ]
    },
    {
      labelName: "Установити поточну дату",
      name: "setCurrDate",
      type: "checkbox",
      id: "SetCurrDate",
      value: true
    },
    {
      labelName: "Показувати підказки",
      name: "showTooltip",
      type: "checkbox",
      id: "ShowTooltip",
      value: false
    },
    {
      labelName: "Зберігати дані користувача при виході",
      name: "saveUser",
      type: "checkbox",
      id: "saveUser",
      value: true
    }
  ]
};

this.createContentSettingsForm = function(data) {
  if (!data || typeof data == "undefined" || self.isStringEmpty(data)) {
    return;
  }

  var settingsControls = data.userDisplaySettingsConfig;
  if (!settingsControls || !settingsControls.length) {
    return;
  }

  //userDisplaySettingsSaved=>userDisplaySettings.userDisplaySettingsConfig

  var html =
    '<div class = "settings-inner">' +
    "<h3>" +
    '<i class="icon icon-lg icon-settings"></i>Налаштування користувача</h3>';

  for (var i = 0; i < settingsControls.length; i++) {
    html += self.selectDataTemplate(settingsControls[i]);
  }

  html += "</div>";
  var modalContainer = $("#modal").find(".modal-container");

  $.when($(modalContainer).html(html)).done(
    self.activateFormControls(modalContainer)
  );

  self.openModal();

  $(modalContainer)
    .find(".checkbox")
    .on("change", function() {
      var elName = $(this).attr("name");

      if (this.checked) {
        userDisplaySettingsSaved[elName] = true;
      } else {
        userDisplaySettingsSaved[elName] = false;
      }
      self.saveToStorage("user-settings", userDisplaySettingsSaved);

      // for (var key in settingsControls) {
      //   if (settingsControls[key].name == "setCurrDate") {
      //     settingsControls[key].value = userDisplaySettingsSaved.setCurrDate;
      //   } else if (settingsControls[key].name == "showTooltip") {
      //     settingsControls[key].value = userDisplaySettingsSaved.showTooltip;
      //   } else if (settingsControls[key].name == "saveUser") {
      //     settingsControls[key].value = userDisplaySettingsSaved.saveUser;
      //   }
      // }
    });

  $(modalContainer)
    .find(".select")
    .on("change", function() {
      var elName = $(this).attr("name");
      var selectedValue = $(this).val();

      userDisplaySettingsSaved[elName] = selectedValue;

      self.saveToStorage("user-settings", userDisplaySettingsSaved);
    });
};

this.updateUserDisplaySettings = function() {

  var obgConfig = userDisplaySettings.userDisplaySettingsConfig,
    options = [];

  for (var key in userDisplaySettingsSaved) {
    for (var i = 0; i < obgConfig.length; i++) {
      if (obgConfig[i].name == key) {

        if ((obgConfig[i].type == "checkbox")) {
          obgConfig[i].value = userDisplaySettingsSaved[key];
        }

        if ((obgConfig[i].type == "select")) {
          options = obgConfig[i].selectOptions;
          for (var t = 0; t < options.length; t++) {
            options[t].Selected = false;
            if (options[t].Value == userDisplaySettingsSaved[key]) {
              options[t].Selected = true;
            }
          }
        }
      }
    }
  }
};

$("#show-settings").on("click", function() {
  self.updateUserDisplaySettings();

  self.createContentSettingsForm(userDisplaySettings);


  self.closeAllOpenLi(headerAccountMenu);
});
