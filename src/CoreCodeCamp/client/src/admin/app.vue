<template>
  <div class="text-left">
    <div class="alert alert-info" v-if="isBusy">
      <i class="fa fa-gear fa-spin"></i> Loading...
    </div>
    <div class="alert alert-warning" v-if="error">{{ error }}</div>
    <div>
      <label>Pick a Camp:</label>
      <select @changed="onCampChange()" v-model="moniker">
        <option :value="camp.moniker" v-for="camp in camps" :key="camp.id">{{ camp.name }}</option>
      </select>
    </div>
    <ul class="nav nav-tabs" role="tabpanel" aria-label="Tabs">
      <li>
        <router-link :to="{ name: 'admin' }">Camp Info</router-link>
      </li>
      <li>
        <router-link :to="{ name: 'users' }">Users</router-link>
      </li>
      <li>
        <router-link :to="{ name: 'talks' }">Talks</router-link>
      </li>
      <li>
        <router-link :to="{ name: 'schedule' }">Scheduling</router-link>
      </li>
      <li>
        <router-link :to="{ name: 'sponsors' }">Sponsors</router-link>
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
  computed: mapState(["error", "camps", "isBusy"]),
  mounted() {
    this.loadCamps().then(() => {
      this.setCampFromMoniker(this.camps[0].moniker);
      this.moniker = this.camps[0].moniker;
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
  background-color: #888;
}
.nav-tabs li a.router-link-active {
  color: #222;
  background-color: #eee;
}
</style>