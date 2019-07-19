<template>
  <div>
    <div class="text-danger" v-if="errorMessage">{{ errorMessage }}</div>
    <div v-if="speaker">
      <h4 v-if="speaker.event">Speaking at the {{ speaker.event.name }}</h4>
      <div class="row text-left">
        <div class="col-md-8">
          <router-link :to="{ name: 'editor' }" class="btn btn-sm btn-primary">Edit Speaker Details</router-link>
          <h3>{{ speaker.name }}</h3>
          <p class="overflow-hidden">
            <img
              class="img-responsive pull-right col-md-3"
              :src="speaker.imageUrl"
              :alt="speaker.name"
            >
            {{ speaker.bio }}
          </p>

          <dl class="dl-horizontal">
            <dt>Title</dt>
            <dd>{{ speaker.title }}</dd>
            <dt>Company</dt>
            <dd>{{ speaker.companyName }}</dd>
            <dt>Company Website</dt>
            <dd>{{ speaker.companyUrl }}</dd>
            <dt>Blog</dt>
            <dd>{{ speaker.blog }}</dd>
            <dt>Website</dt>
            <dd>{{ speaker.website }}</dd>
            <dt>Twitter Handle</dt>
            <dd>{{ speaker.twitter }}</dd>
            <dt>Phone Number</dt>
            <dd>{{ speaker.phoneNumber }}</dd>
            <dt>T-Shirt Size</dt>
            <dd>{{ speaker.tShirtSize }}</dd>
          </dl>
        </div>
        <div class="col-md-4">
          <h3>Talks</h3>
          <div>
            <table class="table table-condensed table-responsive text-sm">
              <thead>
                <tr>
                  <th class="col-xs-8">Name</th>
                  <th class="col-xs-2">Approved?</th>
                  <th class="col-xs-2"></th>
                </tr>
              </thead>
              <tr v-for="(talk, key) in speaker.talks" :key="key">
                <td>{{talk.title}}</td>
                <td class="text-center click-cursor">
                  <i
                    class="fa"
                    v-bind:class="talk.approved ? 'fa-check' : 'fa-times'"
                    :title='talk.approved ? "Approved" : "Not Yet Approved"'
                  ></i>
                </td>
                <td class="btn-group">
                  <router-link
                    class="btn btn-xs btn-primary"
                    :to="{ name: 'talkEditor', params: { id: talk.id }}"
                    title="Edit"
                  >
                    <i class="fa fa-pencil"></i>
                  </router-link>
                  <a class="btn btn-xs btn-danger" @click="onDeleteTalk(talk)" title="Delete">
                    <i class="fa fa-times"></i>
                  </a>
                </td>
              </tr>
            </table>
            <router-link
              :to="{ name: 'talkEditor', params: { id: 'new' } }"
              class="btn btn-sm btn-success"
            >Add Talk</router-link>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import speakerData from "./speakerData";
import routes from "./routes";

export default {
  data() {
    return {
      busy: true,
      errorMessage: "",
      speaker: null
    };
  },
  methods: {
    onDeleteTalk(talk) {
      this.busy = true;
      this.errorMessage = "";
      speakerData
        .deleteTalk(talk)
        .then(() => {}, () => (this.errorMessage = "Failed to delete talk"))
        .finally(() => (this.busy = false));
    }
  },
  mounted() {
    if (!this.speaker) {
      speakerData.getSpeaker().then(
        skr => {
          if (!skr || skr.id == 0) {
            routes.router.push({ name: "editor" });
          } else {
            this.speaker = skr;
          }
        },
        () => (this.errorMessage = "Failed to load speaker")
      );
    }
  }
};
</script>