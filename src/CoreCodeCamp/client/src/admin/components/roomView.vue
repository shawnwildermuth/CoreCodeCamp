<template>
  <div class="room-container">
    <div>
      <div>{{room.name}}</div>
    </div>
    <timeslot v-for="slot in timeslots" :key="slot.id" :timeslot="slot" :room="room" :talk="talkMap[slot.id.toString()]"  />
  </div>
</template>

<script>
import { mapState } from "vuex";
import _ from "lodash";
import Timeslot from "./timeslot";

export default {
  props: ["timeslots", "room"],
  components: { Timeslot },
  computed: {
    ...mapState(["talks"]),
    talkMap() {
      let dict = {};
      _.each(this.talks, t => {
        let ts = _.find(this.timeslots, s => s.time == t.time);
        if (ts) {
          dict[ts.id.toString()] = _.find(this.talks, k => k.time == ts.time && k.room == this.room.name);
        }
      })
      return dict;
    }
  }
};
</script>

<style scoped>
.room-container {
  border: 1px dotted #888;
  margin: 1px;
}
</style>
