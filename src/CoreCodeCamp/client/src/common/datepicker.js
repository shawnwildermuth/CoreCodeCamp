import Vue from "vue";
const $ = require("jquery");
import 'jquery-ui/themes/base/core.css';
import 'jquery-ui/themes/base/theme.css';
import 'jquery-ui/themes/base/datepicker.css';
import 'jquery-ui/ui/core';
import 'jquery-ui/ui/widgets/datepicker';

export default function createDatePicker() {

  Vue.component('datepicker', {
    props: ['value'],
    template: '<input type="text" \
            class="v-datepicker" \
            ref="input" \
            v-bind:value="value | formatDate" \
            v-on:input="$emit(\'input\', $event.target.value)"/>',

    mounted: function () {
      // activate the plugin when the component is mounted.
      if ($.datepicker) {
        $(this.$el).datepicker({
          dateFormat: "mm-dd-yy",
          showOn: "button",
          autoclose: true, 
          buttonImage: "/img/calendar.gif",
          buttonImageOnly: true,
          buttonText: "Select date",
          onClose: this.onClose
        });
      } else {
        console.log("Date Picker isn't install correctly");
      }
    },

    methods: {
      // callback for when the selector popup is closed.
      onClose(date) {
        this.$emit('input', date);
      }
    },
    watch: {
      // when the value fo the input is changed from the parent,
      // the value prop will update, and we pass that updated value to the plugin.
      value(newVal) {
        $(this.el).datepicker('setDate', newVal);
      }
    }
  });
}