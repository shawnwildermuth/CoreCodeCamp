<template>
  <div>
    <div class="row">
      <div class="col-md-9">
        <div>
          <talk-item></talk-item>
        </div>
      </div>
      <div class="col-md-3">
        <button class="btn btn-sm pull-right">
          <i class="fa fa-times"></i>
        </button>
        <label-edit :text="theTime" @text-updated="onTimeUpdated"></label-edit>
      </div>
    </div>
  </div>
</template>

<script>
import labelEdit from "./labelEdit";
import moment from "moment";
import { mapActions } from "vuex";
import talkItem from "./talkItem";

export default {
  props: ["timeSlot"],
  components: { labelEdit, talkItem },
  methods: {
    ...mapActions(["updateTimeslot"]),
    onTimeUpdated(text) {
      let time = moment(text, "hh:mma");
      let date = moment(this.timeSlot.time).set({
        hour: time.hour(),
        minute: time.minute()
      });
      this.updateTimeslot({ timeslot: this.timeSlot, value: date });
    }
  },
  computed: {
    theTime: {
      get: function() {
        return moment(this.timeSlot.time).format("hh:mma");
      }
    }
  }
};
</script>

<style>
</style>
