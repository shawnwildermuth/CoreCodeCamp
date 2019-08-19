<template>
  <div>
    <talk-modal></talk-modal>
    <h3>Schedule</h3>
    <div class="row">
      <div class="col-md-6 sticky-container">
        <div>
          Sort/Filter:
          <button class="btn btn-sm btn-success" @click="onSort('title')" :disabled="sort == 'title'">Title</button>
          <button class="btn btn-sm btn-success" @click="onSort('speaker.name')">Speaker</button>
          <button class="btn btn-sm btn-success" @click="onSort('category')" :disabled="sort == 'category'">Category</button>
        </div>
        <drop @drop="onDrop" class="schedule-talk-list">
          <talk-item class="talk-item"
                     :talk="talk"
                     v-for="talk in unassignedTracks"
                     :key="talk.name" />
        </drop>
      </div>
      <div class="col-md-6">
        <room-view v-for="room in rooms"
                   :key="room.id"
                   :timeslots="timeslots"
                   :room="room"
                   class="room-container"></room-view>
      </div>
    </div>
  </div>
</template>

<script>
  import { mapState, mapActions } from "vuex";
  import TalkItem from "./components/talkItem";
  import TalkModal from "./components/talkModal";
  import RoomView from "./components/roomView";
  import _ from "lodash";

  export default {
    data() {
      return {
        dragValid: true,
        sort: "category"
      }
    },
    computed: {
      unassignedTracks() {
        return _.sortBy(this.talks, this.sort)
          .filter(t => !t.room);
      },
      ...mapState(["rooms", "talks", "timeslots"])
    },
    components: {
      TalkItem,
      TalkModal,
      RoomView
    },
    methods: {
      ...mapActions(["unassignTalk"]),
      onDrop(talk) {
        console.log(`Talk dropped: ${talk.title}`)
        this.unassignTalk(talk);
      },
      onSort(sort) {
        this.sort = sort;
      }
    }
  };
</script>
<style scoped>
  .sticky-container {
    position: -webkit-sticky;
    position: sticky;
    top: 50px;
  }

  .schedule-talk-list {
    overflow-y: scroll;
    overflow-x: hidden;
    height: 600px;
    border: 1px solid #ccc;
    padding: 1px;
  }

    .schedule-talk-list .talk-item {
      padding: 1px;
      border: 1px #666 dotted;
    }
</style>