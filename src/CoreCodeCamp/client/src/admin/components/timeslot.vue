<template>
  <div class="time-slot">
    <drop @drop="onDrop">
      <div class="row">
        <div class="col-md-2">
          <div class="pull-right"><small>{{ timeslot.time | formatTime }}</small></div>
        </div>
        <div class="col-md-10">
          <div>
            <talk-item v-if="talk" :talk="talk"></talk-item>
          </div>
        </div>
      </div>
    </drop>
  </div>
</template>

<script>
import talkItem from "./talkItem";
import { mapState, mapActions } from "vuex";

export default {
  props: ["timeslot", "room", "talk"],
  components: { talkItem },
  methods: {
    ...mapActions(["assignRoom","swapRooms"]),
    onDrop(talk) {
      if (this.talk != null) {
        this.swapRooms({droppedTalk: talk, existingTalk: this.talk, room: this.room, timeslot: this.timeslot});
      } else {
        this.assignRoom({ talk, room: this.room, timeslot: this.timeslot});
      }
    }
  },
  computed: {
    ...mapState(["talks"])
  }
};
</script>

<style scoped>
.time-slot {
  margin: 0 1px;
  border: 1px solid #eee;
}
</style>
