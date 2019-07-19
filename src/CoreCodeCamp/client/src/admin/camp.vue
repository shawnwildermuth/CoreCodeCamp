<template>
  <div>
    <h4>{{ currentCamp.name }}</h4>
      <dl class="dl-horizontal">
        <dt>Date:</dt>
        <dd>{{ currentCamp.eventDate | formatDate }}</dd>
        <dt>Length:</dt>
        <dd>{{ currentCamp.eventLength }}</dd>
        <dt>CFP Opened:</dt>
        <dd>{{ currentCamp.callForSpeakersOpened | formatDate }}</dd>
        <dt>CFP Closed:</dt>
        <dd>{{ currentCamp.callForSpeakersClosed | formatDate }}</dd>
        <dt>Total Speakers:</dt>
        <dd>{{ summary.speakers }}</dd>
        <dt>Talks Submitted:</dt>
        <dd>{{ summary.talks }}</dd>
        <dt>Talks Approved:</dt>
        <dd>{{ summary.approved }}</dd>
        <dt>Total Sponsors:</dt>
        <dd>{{ summary.sponsors }}</dd>
        <dt>Total Users:</dt>
        <dd>{{ summary.users }}</dd>
        <dt></dt>
        <dd>
          <a
            :href="`/${currentCamp.moniker}/admin/speakerlist`"
          >Download Approved Speaker List (.csv)</a>
        </dd>
        <dt></dt>
        <dd>
          <a
            :href="`/${currentCamp.moniker}/admin/submissionlist`"
          >Download Submitted Talks List (.csv)</a>
        </dd>
      </dl>
      <address v-if="currentCamp.location">
        <strong>{{ currentCamp.location.facility }}</strong>
        <br />
        {{ currentCamp.location.address1 }}
        <br />
        <span v-if="currentCamp.location.address2">
          {{ currentCamp.location.address2 }}
          <br />
        </span>
        {{ currentCamp.location.city }}, {{ currentCamp.location.stateProvince }} {{ currentCamp.location.postalCode }}
        <br />
        {{ currentCamp.location.country }}
        <br />
      </address>
      <div>
        <router-link
          class="btn btn-info"
          to="/camp/edit/edit"
          :camp="this.$store.state.currentCamp"
        >
          <i class="fa fa-pencil"></i> Edit Camp
        </router-link>&nbsp;
        <router-link class="btn btn-info" to="/camp/edit/new" :camp="this.$store.state.currentCamp">
          <i class="fa fa-gift"></i> New Camp
        </router-link>
      </div>
  </div>
</template>
<script>
import { mapState } from "vuex";

export default {
  computed: mapState(["currentCamp", "summary"]),
};
</script>