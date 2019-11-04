this.setGrid = function(el) {

    var gridChildren = $(el).children(),
    gridChildrenLength = gridChildren.length,
    currGridChild,
    currGrid,
    gridColumns = "",
    gridTemplate = " ";

  if (!gridChildrenLength) {
    return;
  }

  for (var i = 0; i < gridChildrenLength; i++) {
    currGridChild = gridChildren[i];

    if ($(currGridChild).data("grid")) {
      currGrid = $(currGridChild).data("grid");
      currGrid = +currGrid;
    } else {
      currGrid = 1;
    }

    if (currGrid > 3) {
      currGrid = 3;
    }
    if (currGrid < 1) {
      currGrid = 1;
    }

    gridTemplate = gridTemplate + "_" + i + " ";
    gridColumns += "1fr ";
    if (currGrid > 1) {
      gridTemplate = gridTemplate + "_" + i + " ";
      gridColumns += "1fr ";
    }
    if (currGrid > 2) {
      gridTemplate = gridTemplate + "_" + i + " ";
      gridColumns += "1fr ";
    }
  
  }

  $(el).css({
    "grid-template-columns": gridColumns,
    "grid-template-areas": '"' + gridTemplate + '"'
  });

  if ($(el).closest(".content-list-header")) {
    $(el)
      .closest(".content-list-header")
      .siblings(".content-list")
      .each(function() {
        $(this)
          .find(".grid-container-nested")
          .each(function() {
            $(this).css({
              "grid-template-columns": gridColumns,
              "grid-template-areas": '"' + gridTemplate + '"'
            });
          });
      });
  }
  self.manageGridContent();
};


this.manageGridContent = function() {
  if (!$("div").is(".grid")) {
    return;
  }

  var grid = $(".grid"),
    gridLength = grid.length,
    currGrid,
    currGridItem,
    currGridItemWidth,
    currGridItemHeight,
    currGridInner,
    currGridInnerWidth,
    currGridInnerHeight;

  for (var i = 0; i < gridLength; i++) {
    currGrid = grid[i];
    currGridItem = $(currGrid).children(".grid-inner");
    currGridItem.removeClass("grid-hover");
    currGridItem.removeClass("grid-hover-height");
    currGridInner = currGridItem.children("p:not(.mask-money-dec)");
    
    

    currGridItemWidth = currGridItem.outerWidth();
    currGridInnerWidth = currGridInner.width();

    currGridItemHeight = currGridItem.outerHeight();
    currGridInnerHeight = currGridInner.height();
    
    if(currGridInner.length>1){
      for (var k = 0; k < currGridInner.length; k++){
        var gridInnerItem = currGridInner[k];
        var gridInnerItemWidth = $(gridInnerItem).width();
        var gridInnerItemHeight = $(gridInnerItem).height();
        if(currGridInnerWidth < gridInnerItemWidth){
          currGridInnerWidth = gridInnerItemWidth;
        }
        currGridInnerHeight += gridInnerItemHeight;
      }
    }
    currGridItem.removeClass("grid-hover");
    currGridItem.removeClass("grid-hover-height");
    
    if (currGridInnerHeight > currGridItemHeight && currGridInnerWidth > currGridItemWidth) {
      currGridItem.addClass("grid-hover-height");
    } else if (currGridInnerHeight > currGridItemHeight) {
      currGridItem.addClass("grid-hover-height");
    } else if (currGridInnerWidth > currGridItemWidth) {
      currGridItem.addClass("grid-hover");
    } else {
      currGridItem.removeClass("grid-hover");
      currGridItem.removeClass("grid-hover-height");
    }
  }
};


this.findGrids = function(el) {
  $(el)
    .find(".grid-container")
    .each(function() {
      self.setGrid($(this));

      //find content-list-link
      setTimeout(function () {
        if ($("a").is(".content-list-link")) {
          self.addContentListLink(body);
        }
      }, 0);
    });
};

if ($("div").is(".grid-container")) {
  $(".grid-container").each(function() {
    self.setGrid($(this));
  });
}

//set content-list-link href attr
this.addContentListLink  = function (el) {
  $(el)
    .find(".content-list-link")
    .each(function () {
      var link = $(this).siblings('.content-list-edit').children('.btn-link').attr('href');
    $(this).attr('href', link);
    });
};




