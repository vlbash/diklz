//todo list
/*
1. при сохранении предфильтров 
1.1. инпуты 
1.1.1.- если не датапикер  - тупо сохранить
1.1.2. - если датапикер:
1.1.2.1 - если выбрана дата - записать дату
1.1.2.2. - если выбран селект - записать селект
различия по имени, наверное, или по формату

2. при выборе 
2.1.
2.1.1. датапикеры - если дата "с  - по" добавить проверку, чтоб дата "по" была не больше, чем дата "с"
2.1.2. - при двойном формате даты предусмотреть, что первой дата может быть введена, как в первом инпуте, тка и во втором

3. предфильтра после перемещения удаляется не тот фильтр

4. клик по предфильтру




*/

var FormObject = function (el) {
     var _this = this;
     this.form = el;
     this.filters = [];
     this.filled = false;
     this.formWrapper = $(_this.form).closest('.content-search');
     this.formHolder = $(_this.form).find('.content-search-form-holder');
     this.formPreset = $(_this.form).find('.content-search-form-preset');
     this.formSwitcher = $(_this.formWrapper).find('.content-search-switcher');
     this.formPresetSwitcher = $(_this.formWrapper).find('.content-preset-switcher');

     this.mainInput = $(_this.formWrapper).find('.content-search-main-input') || false;
     this.mainText = $(_this.formWrapper).find('.content-search-main-text') || false;
     this.shownFilters = [];
     this.hasFields = false;
     this.hasPresetFilters = false;
     this.dirty = false;

     this.submitBtn = $(_this.formHolder).find('.search-form-btn-submit');
     this.saveFilterBtn = $(_this.formHolder).find('.search-form-btn-save-filter');
     this.clearFormBtn = $(_this.formHolder).find('.search-form-btn-clean');

     this.savePresetFilterForm = $(_this.formHolder).find('.preset-filter-wrapper');
     this.savePresetFilterNameInput = $(_this.savePresetFilterForm).find('.preset-filter-name-input');
     this.savePresetFilterDefaultCheckbox = $(_this.savePresetFilterForm).find('.preset-filter-default-checkbox');
     this.savePresetFilterSaveBtn = $(_this.savePresetFilterForm).find('.preset-filter-save');
     this.savePresetFilterCancelBtn = $(_this.savePresetFilterForm).find('.preset-filter-cancel');

     this.savePresetFilterFormOpen = false;

     this.parametersFilterHolder = $(_this.formWrapper).find('.content-filter-parameters');
     this.clearAllFiltersBtn = $(_this.parametersFilterHolder).find('.content-filter-parameters-clear-all');

};