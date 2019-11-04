this.tooltipEl = function (el) {
     // if(!userDisplaySettingsSaved.ShowTooltip){
     //      return;
     // }
     $(el).attr('title', $(el).data('title'));
     $(el).removeData('title');

     if ($(el).data('tooltipleft')) {
          $(el).tooltip({
               position: {
                    my: "right-15 center",
                    at: "left center"
               },
               classes: {
                    "ui-tooltip": "tooltip tooltip-left"
               }
          });
     } else if ($(el).data('tooltipright')) {
          $(el).tooltip({
               position: {
                    my: "left+15 center",
                    at: "right center"
               },
               classes: {
                    "ui-tooltip": "tooltip tooltip-right"
               }
          });
     } else {
          $(el).tooltip();
     }
}


this.setTooltips = function () {
     $('[data-title]').each(function () {

          self.tooltipEl($(this));
     })
}

this.setToolTipsForChild = function (el) {
     $(el).find('[data-title]').each(function () {

          self.tooltipEl($(this));
     })
}



if ($('*').is('[data-title]')) {
     self.setTooltips();
}