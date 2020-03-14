<template>
  <div class="text-left">
    <div class="wait-cursor" v-if="isBusy">
      <div class="message">
        <i class="fa fa-gear fa-spin"></i> Please Wait...
      </div>
    </div>
    <div class="alert alert-warning" v-if="errorState">{{ errorState }}</div>
    <div>
      <label>Pick a Camp:</label>
      <select @changed="onCampChange()" v-model="moniker">
        <option :value="camp.moniker" v-for="camp in camps" :key="camp.id">{{ camp.name }}</option>
      </select>
    </div>
    <ul class="nav nav-tabs" role="tabpanel" aria-label="Tabs">
      <li class="nav-item">
        <router-link :to="{ name: 'admin' }" class="nav-link">Camp Info</router-link>
      </li>
      <li class="nav-item">
        <router-link :to="{ name: 'users' }" class="nav-link">Users</router-link>
      </li>
      <!--<li class="nav-item">
        <router-link :to="{ name: 'talks' }" class="nav-link">Talks</router-link>
      </li>
      <li class="nav-item">
        <router-link :to="{ name: 'scheduleSetup' }" class="nav-link">Rooms and Timeslots</router-link>
      </li>
      <li class="nav-item">
        <router-link :to="{ name: 'schedule' }" class="nav-link">Talk Scheduling</router-link>
      </li>-->
      <li class="nav-item">
        <router-link :to="{ name: 'sponsors' }" class="nav-link">Sponsors</router-link>
      </li>
    </ul>
    <div class="panel panel-default">
      <div class="panel-body">
        <router-view></router-view>
      </div>
    </div>
  </div>
</template>
<script>
import { mapState, mapActions } from "vuex";

export default {
  data: () => {
    return {
      moniker: ""
    };
  },
  computed: mapState(["errorState", "camps", "isBusy"]),
  mounted() {
    this.loadCamps().then(() => {
      let currentMoniker = this.camps[0].moniker;
      if (localStorage && localStorage.hasOwnProperty("moniker")) {
        currentMoniker = localStorage.getItem("moniker");
      }

      this.setCampFromMoniker(currentMoniker);
      this.moniker = currentMoniker;
    });
  },
  watch: {
    moniker(newValue) {
      this.setCampFromMoniker(newValue);
    }
  },
  methods: {
    ...mapActions(["setCampFromMoniker", "loadCamps"]),
    onCampChange() {
      this.setCampFromMoniker(this.moniker);
    },
    testClick() {
      this.$forceUpdate();
    }
  }
};
</script>
<style>
.nav-tabs > li {
  color: #ffffff;
}
.nav-tabs > li a {
  color: #ffffff;
  background-color: #4444ff;
}
.nav-tabs li a.router-link-active.router-link-exact-active {
  color: #222;
  background-color: #aaaaff;
}

.wait-cursor {
  background: rgba(0, 0, 0, 0.8);
  z-index: 999;
  position: fixed; /* Sit on top of the page content */
  width: 100%; /* Full width (cover the whole page) */
  height: 100%; /* Full height (cover the whole page) */
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  cursor: pointer; /* Add a pointer on hover */
}

.wait-cursor .message {
  font-size: 33px;
  color: #eee;
  text-align: center;
  margin-top: 500px;
}
</style>