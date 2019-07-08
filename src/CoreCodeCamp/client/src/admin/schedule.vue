<template>
  <div>
    <h3>Schedule</h3>
    <p>Will be completed by close of Call for Speakers</p>
    <!-- <div class="col-md-12">
      <div v-show="busy"><i class="fa fa-spin fa-spinner"></i> Please wait...</div>
      <span v-show="errorMessage" class="text-danger">{{ errorMessage }}</span>
      <span v-show="userMessage" class="text-success">{{ userMessage }}</span>
      <div class="row">
        <div class="col-lg-12">Speakers: {{ summary.speakers }} | All Talks: {{ summary.talks }} | Approved Talks: {{ summary.approved }}</div>
        <table class="table table-striped table-bordered table-condensed">
          <thead>
            <tr>
              <td class="col-lg-4">
                <span :class="{ 'text-bold': sort == 'title' }">
                  Title
                </span>
              </td>
              <td class="col-lg-2">
                <span @@click="onSort('category')" title="Sort" class="click-cursor" :class="{ 'text-bold': sort == 'category' }">
                  Category
                  <i class="fa" :class="{ 'icon-enabled': sort == 'category', 'fa-sort-asc': sort == 'category' && sortAsc, 'fa-sort-desc': sort == 'category' && !sortAsc }"></i>
                </span>
              </td>
              @*
              <td class="col-lg-1">
                <span @@click="onSort('track')" title="Sort" class="click-cursor" :class="{ 'text-bold': sort == 'track' }">
                  Track
                  <i class="fa" :class="{ 'icon-enabled': sort == 'track', 'fa-sort-asc': sort == 'track' && sortAsc, 'fa-sort-desc': sort == 'track' && !sortAsc }"></i>
                </span>
              </td>*@
              <td class="col-lg-1">
                <span @@click="onSort('room')" title="Sort" class="click-cursor" :class="{ 'text-bold': sort == 'room' }">
                  Room
                  <i class="fa" :class="{ 'icon-enabled': sort == 'room', 'fa-sort-asc': sort == 'room' && sortAsc, 'fa-sort-desc': sort == 'room' && !sortAsc }"></i>
                </span>
              </td>
              <td class="col-lg-1">
                <span @@click="onSort('time')" title="Sort" class="click-cursor" :class="{ 'text-bold': sort == 'time' }">
                  Timeslot
                  <i class="fa" :class="{ 'icon-enabled': sort == 'time', 'fa-sort-asc': sort == 'time' && sortAsc, 'fa-sort-desc': sort == 'time' && !sortAsc }"></i>
                </span>
              </td>
              <td class="col-lg-1">
                <span @@click="onSort('approved')" title="Sort" class="click-cursor" :class="{ 'text-bold': sort == 'approved' }">
                  Aprv'd
                  <i class="fa" :class="{ 'icon-enabled': sort == 'approved', 'fa-sort-asc': sort == 'approved' && sortAsc, 'fa-sort-desc': sort == 'approved' && !sortAsc }"></i>
                </span>
              </td>
              <td class="col-lg-1">
                <span @@click="onSort('votes')" title="Sort" class="click-cursor" :class="{ 'text-bold': sort == 'votes'}">
                  Votes
                  <i class="fa" :class="{ 'icon-enabled': sort == 'votes', 'fa-sort-asc': sort == 'votes' && sortAsc, 'fa-sort-desc': sort == 'votes' && !sortAsc }"></i>
                </span>
              </td>
              <td class="col-lg-1"></td>
            </tr>
          </thead>
          <tbody v-for="talk in talks">
            <tr>
              <td>{{talk.title}}</td>
              <td>{{talk.category}}</td>
              @*
              <td>
                <select v-model="talk.track" @@change="onTrackChanged(talk, $event)">
                  <option v-for="t in tracks">{{ t.name }}</option>
                </select>
              </td>*@
              <td>
                <select v-model="talk.room" @@change="onRoomChanged(talk, $event)">
                  <option v-for="t in rooms">{{ t.name }}</option>
                </select>
              </td>
              <td>
                <select v-model="talk.time" @@change="onTimeChanged(talk, $event)">
                  <option v-for="t in timeSlots" :value="t.time">{{ t.time | formatTime }}</option>
                </select>
              </td>
              <td>{{ talk.approved ? "Yes" : "No" }}</td>
              <td>{{ talk.votes }}</td>
              <td>
                <div class="btn-group ">
                  <button class="btn btn-sm btn-primary" @@click="onToggleApproved(talk)" title="Toggle Approved"><i class="fa fa-check"></i></button>
                  <button class="btn btn-sm btn-danger" @@click="onDelete(talk)" title="Delete"><i class="fa fa-remove"></i></button>
                </div>
              </td>
            </tr>
            <tr>
              <td>Speaker: <a :href="talk.speaker.speakerLink" target="_blank">{{talk.speaker.name}}</a> (<a :href="'mailto:' + talk.speaker.email + '?subject=Atlanta Code Camp'">email</a>)</td>
              <td class="table-abstract" colspan="6"><p>{{ talk.abstract }}</p></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div> -->
  </div>

</template>
<script>
// import mount from "../common/mount";
// import Vue from "vue";
// import dataService from "../common/dataService";

// export default {
//   data: {
//     busy: true,
//     talks: [],
//     rooms: [],
//     timeSlots: [],
//     tracks: [],
//     errorMessage: "",
//     userMessage: "",
//     sort: "",
//     sortAsc: true,
//     summary: {
//       speakers: 0,
//       approved: 0,
//       talks: 0
//     }
//   },
//   methods: {
//     showError(err) {
//       this.errorMessage = err;
//     },
//     setMsg(text) {
//       this.userMessage = text;
//       window.setTimeout(() => this.userMessage = "", 5000);
//     },
//     onTrackChanged(talk, $event) {
//       this.busy = true;
//       this.userMessage = "";
//       this.errorMessage = "";
//       let value = this.tracks[$event.target.selectedIndex];
//       dataService.updateTalkTrack(talk, value.name)
//         .then(() => this.setMsg("Saved..."),
//           () => this.showError("Failed to update talk"))
//         .finally(() => this.busy = false);
//     },
//     onRoomChanged(talk, $event) {
//       this.busy = true;
//       this.userMessage = "";
//       this.errorMessage = "";
//       let value = this.rooms[$event.target.selectedIndex];
//       dataService.updateTalkRoom(talk, value.name)
//         .then(result => {
//           this.setMsg("Saved...");
//         },
//           e => {
//             this.showError("Failed to update talk");
//           })
//         .finally(() => this.busy = false);
//     },
//     onTimeChanged(talk, $event) {
//       this.busy = true;
//       this.userMessage = "";
//       this.errorMessage = "";
//       let value = this.timeSlots[$event.target.selectedIndex];
//       dataService.updateTalkTime(talk, value.time)
//         .then(() => this.setMsg("Saved..."),
//           () => this.showError("Failed to update talk"))
//         .finally(() => this.busy = false);
//     },
//     onSort(sort) {
//       if (sort == this.sort) {
//         if (this.sortAsc) this.sortAsc = false;
//         else {
//           this.sort = "title"; // Reset to default sort
//           this.sortAsc = true;
//         }
//       } else {
//         this.sort = sort;
//         this.sortAsc = true;
//       }

//       // do the sort
//       this.talks = this.talks.sort((a, b) => {
//         if (a[this.sort] == b[this.sort]) return 0;
//         if (a[this.sort] < b[this.sort]) return this.sortAsc ? -1 : 1;
//         else return this.sortAsc ? 1 : -1;
//       });
//     },
//     updateSummary() {
//       this.summary.approved = this.talks.filter(t => t.approved).length;
//       this.summary.talks = this.talks.length;
//       this.summary.speakers = this.talks.map(t => t.speaker.name).filter((x, i, s) => s.indexOf(x) === i).length;
//     },
//     onDelete(talk) {
//       this.busy = true;
//       dataService.deleteTalk(talk.id)
//         .then(() => {
//           this.talks.splice(this.talks.indexOf(talk), 1);
//           this.updateSummary();
//         }, e => this.showError("Failed to delete talk"))
//         .finally(() => this.busy = false);
//     },

//     onToggleApproved(talk) {
//       this.busy = true;
//       dataService.toggleApproved(talk)
//         .then(result => {
//           talk.approved = !talk.approved;
//           this.updateSummary();
//         }, e => this.showError("Failed to toggle approved flag"))
//         .finally(() => this.busy = false);
//     }

//   },
//   mounted() {
//     Vue.Promise.all([
//       dataService.getAllTalks(),
//       dataService.getRooms(),
//       dataService.getTimeSlots(),
//       dataService.getTracks()
//     ])
//       .then(result => {
//         this.talks = result[0].data;
//         this.rooms = result[1].data;
//         this.timeSlots = result[2].data;
//         this.tracks = result[3].data;
//         this.updateSummary();
//       },  () => {
//         this.errorMessage = "Failed to load data";
//       })
//       .finally(() => {
//         this.busy = false;
//       });

//   }
// };
</script>