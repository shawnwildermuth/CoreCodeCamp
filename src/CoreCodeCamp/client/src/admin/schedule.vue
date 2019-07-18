<template>
  <div>
    <schedule-toolbar></schedule-toolbar>
    <h3>Schedule</h3>
    <div class="row">
      <div class="col-md-6">
        <draggable
          class="schedule-talk-list drop-target"
          v-model="talks"
          v-bind="dragOptions"
          group="talks"
          :move="onMoveToUnassigned"
        >
          <talk-item class="talk-item" :talk="talk" v-for="talk in talks" :key="talk.name" />
        </draggable>
      </div>
      <div class="col-md-6">
        <div v-for="room in rooms" :key="room.id" class="room-container">
          <div>
            <span class="pull-right">
              <button class="btn btn-sm"><i class="fa fa-times" @click="onRoomDeleted(room)"></i></button>
            </span>
            <label-edit :text="room.name" @text-updated="onRoomUpdated" :src="room" />
          </div>
          <draggable
            class="drop-target"
            group="talks"
            v-bind="dragOptions"
            :move="onMoveToTimeslot"
            :drop="onDropOnTimeslot">
            <timeslot v-for="slot in timeSlots" :key="slot.id" :timeSlot="slot" />
          </draggable>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { mapState, mapActions } from "vuex";
import TalkItem from "./components/talkItem";
import ScheduleToolbar from "./components/scheduleToolbar";
import Timeslot from "./components/timeslot";
import draggable from "vuedraggable";
import LabelEdit from "./components/labelEdit";

export default {
  // data: () => {
  //   return {

  //   };
  // },
  computed: {
    dragOptions() {
      return {
        dropzoneSelector: ".",
        draggableSelector: "talk-item"
      };
    },
    ...mapState(["tracks", "rooms", "talks", "timeSlots"])
  },
  components: {
    TalkItem,
    LabelEdit,
    ScheduleToolbar,
    Timeslot,
    draggable
  },
  methods: {
    ...mapActions(["updateRoomName", "deleteRoom"]),
    onMoveToUnassigned({ relatedContext, draggedContext }) {
      // Allow if assigned
      return false;
    },
    onMoveToTimeslot({ relatedContext, draggedContext }) {
      // allow if unassigned
      const payload = draggedContext.element;
      console.log(`Dropping: ${payload.title}`);
      return !payload.room || !payload.time;
    },
    onDropOnTimeslot() {},
    onRoomUpdated(text, src) {
      this.updateRoomName({room: src, name: text});
    },
    onRoomDeleted(room) {
      this.deleteRoom(room);
    }
  }
};
</script>
<style>
.schedule-talk-list {
  overflow-y: scroll;
  height: 400px;
  border: 1px solid #ccc;
  padding: 1px;
}

.schedule-talk-list .talk-item {
  padding: 1px;
  border: 1px #666 dotted;
}

.room-container {
   border: 1px dotted #888;
   margin: 1px;
}
</style>