<template>
  <div class="col-md-6 col-md-offset-3">
    <h3>Rooms</h3>
    <div class="item-list">
      <div v-for="room in rooms" :key="room.id">
        <button class="btn btn-sm pull-right">
          <i class="fa fa-times red" @click="onDeleteRoom(room)"></i>
        </button>
        <label-edit :text="room.name" @text-updated="onRoomChange" :src="room"></label-edit>
      </div>
    </div>
    <button class="btn btn-success"  @click="addRoom">
      <i class="fa fa-plus"></i> Add New Room
    </button>
    <h3>Time Slots</h3>
    <div class="item-list">
      <div v-for="slot in timeSlots" :key="slot.id">
        <button class="btn btn-sm pull-right"><i class="fa fa-times red" @click="onDeleteSlot(slot)"></i></button>
        <label-edit :text="formatTime(slot.time)" @text-updated="onSlotChange" :src="slot"></label-edit>
      </div>
    </div>
    <button class="btn btn-success"  @click="addTimeslot">
      <i class="fa fa-plus"></i> Add New Timeslot
    </button>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
import labelEdit from "./labelEdit";
import moment from "moment";

export default {
  components: { labelEdit },
  computed: mapState(["rooms", "timeSlots"]),
  methods: {
    ...mapActions(["updateRoomName", "deleteRoom", "addRoom", "addTimeslot", "updateTimeslot", "deleteTimeslot"]),
    onDeleteRoom(room) {
      this.deleteRoom(room);
    },
    onRoomChange(text, src) {
      this.updateRoomName({ room: src, name: text });
    },
    onDeleteSlot(slot) {
      this.deleteTimeslot(slot);
    },
    onSlotChange(text, src) {
      const date = moment(text, "hh:mma");
      const day = moment(src.time);
      const time = day.set({hour: date.hour(), minute: date.minute()});
      this.updateTimeslot({ timeslot: src, value: time });
    },
    formatTime(time) {
      return moment(time).format("hh:mma");
    }
  }
};
</script>

<style>
.item-list {
  border: 1px dotted #888;
  padding: 1px;
}
</style>
