$(document).ready(function () {
    var Utils = $.fn.select2.amd.require('select2/utils');
    var Dropdown = $.fn.select2.amd.require('select2/dropdown');
    var DropdownSearch = $.fn.select2.amd.require('select2/dropdown/search');
    var CloseOnSelect = $.fn.select2.amd.require('select2/dropdown/closeOnSelect');
    var AttachBody = $.fn.select2.amd.require('select2/dropdown/attachBody');

    var dropdownAdapter = Utils.Decorate(Utils.Decorate(Utils.Decorate(Dropdown, DropdownSearch), CloseOnSelect), AttachBody);

    window.select2Common = {

        Select2General: function (id, dotNetHelper) {
            $('#' + id).select2({
                dropdownAdapter: dropdownAdapter,                 //for search field
                minimumResultsForSearch: 0,
                multiple: false
            });

            $('#' + id).on('select2:select', function (event) {
                var selectedValues = $(this).val();

                var selectedData = selectedValues ? selectedValues : 0;
                dotNetHelper.invokeMethodAsync('HandleSelection', selectedData);
            });
        },

        Select2GeneralMultiple: function (id, placeHolder, dotNetHelper) {
            $('#' + id).select2({
                dropdownAdapter: dropdownAdapter,                 //for search field
                minimumResultsForSearch: 0,
                placeholder: {
                    id: '-1', // the value of the option
                    text: placeHolder
                },
                allowClear: true,
                multiple: true
            });

            $('#' + id).on('select2:opening select2:closing', function (event) {
                var $searchfield = $(this).parent().find('.select2-search__field');
                $searchfield.prop('disabled', false);
            });

            $('#' + id).on('select2:select', function (event) {
                var selectedValues = event.params.data.id;
                dotNetHelper.invokeMethodAsync('HandleSelection', selectedValues);
            });

            $('#' + id).on('select2:unselecting', function (event) {
                var unselectedValue = event.params.args.data.id;
                dotNetHelper.invokeMethodAsync('RemoveSelection', unselectedValue);
            });
        },

        SetSelectedValue: function (id, values) {
            $('#' + id).select2().val(values).trigger('change');
        },

        GetSelectedValues: function (id) {
            var selectedValues = $('#' + id).select2().val();

            return selectedValues;
        }
    };
});