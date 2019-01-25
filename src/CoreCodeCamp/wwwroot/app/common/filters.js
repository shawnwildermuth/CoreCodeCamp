import Vue from "vue";
import moment from "moment";

export default function createFilters() {

  Vue.filter('formatDate', function (value) {
    let dt = null;
    if (typeof value === 'string') {
      dt = moment(value);
      if (!dt.isValid()) {
        dt = moment(value, 'MM-DD-YYYY');
      }
    } else if (value) {
      dt = moment(String(value));
    }

    if (dt) {
      return dt.format('MM-DD-YYYY');
    }
  });
  Vue.filter('formatTime', function (value) {
    if (value) {
      return moment(String(value)).format('hh:mm a')
    }
  });

}
