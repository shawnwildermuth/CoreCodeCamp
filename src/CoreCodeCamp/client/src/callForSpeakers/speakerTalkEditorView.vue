<template>
  <div>
    <div>
      <h3>Talk Editor</h3>
    </div>
    <div class="text-danger" v-html="errorMessage" v-if="errorMessage"></div>

    <form novalidate v-if="talk" ref="theForm">
      <div class="col-md-6 col-md-offset-3 text-left">
        <div class="row">
          <div v-if="busy">
            <i class="fa fa-spin fa-spinner"></i> Please wait...
          </div>
          <div class="form-group col-md-12" v-bind:class="{ error: errors.has('title') }">
            <label class="text-danger">Title</label>
            <input
              v-model="talk.title"
              class="form-control"
              placeholder="Talk Title"
              name="title"
              v-validate="'required|min:10'"
            >
            <div class="help-block">{{ errors.first('title') }}</div>
          </div>
          <div class="form-group col-md-12" v-bind:class="{ error: errors.has('abstract') }">
            <label class="text-danger">Abstract *</label>
            <textarea
              v-model="talk.abstract"
              cols="40"
              rows="6"
              class="form-control"
              name="abstract"
              v-validate="'required|min:10|max:1024'"
              placeholder="Two or three sentences describing your talk..."
            ></textarea>
            <div class="help-block">{{ errors.first('abstract') }}</div>
          </div>
          <div class="form-group col-md-12">
            <label>Prerequisites</label>
            <input
              v-model="talk.prerequisites"
              class="form-control"
              placeholder="e.g. ASP.NET"
              name="prerequisites"
            >
          </div>
          <div class="form-group col-md-6" v-bind:class="{ error: errors.has('audience') }">
            <label class="text-danger">Audience *</label>
            <select
              v-model="talk.audience"
              class="form-control"
              name="audience"
              v-validate="'required'"
            >
              <option selected>Developers</option>
              <option>IT Professionals</option>
              <option>Managers</option>
              <option>Anyone</option>
            </select>
            <div class="help-block">{{ errors.first('audience') }}</div>
          </div>
          <div class="form-group col-md-6" v-bind:class="{ error: errors.has('level') }">
            <label class="text-danger">Talk Level *</label>
            <select v-model="talk.level" name="level" class="form-control" v-validate="'required'">
              <option>Beginner</option>
              <option>Intermediate</option>
              <option>Advanced</option>
              <option>Expert</option>
            </select>
            <div class="help-block">{{ errors.first('level') }}</div>
          </div>
          <div class="form-group col-md-6" v-bind:class="{ error: errors.has('category') }">
            <label class="text-danger">Category *</label>
            <select
              v-model="talk.category"
              name="category"
              class="form-control"
              v-validate="'required'"
            >
              <option>General Discussion</option>
              <option>Client Development</option>
              <option>Web Development</option>
              <option>Database Development</option>
              <option>Cloud Development</option>
              <option>Design UI/UX</option>
              <option>Professional Development</option>
              <option>Career Advancement</option>
              <option>IT Topics</option>
            </select>
            <div class="help-block">{{ errors.first('category') }}</div>
          </div>
          <div class="form-group col-md-12">
            <input
              type="submit"
              class="btn btn-success btn-lg"
              value="Save"
              @click.prevent="onSave()"
              v-bind:disabled="errors.any() || isPristine">
            <router-link :to="{ name: 'info'}" class="btn btn-default btn-lg">Cancel</router-link>
          </div>
        </div>
      </div>
    </form>
  </div>
</template>

<script>
import Vue from "vue";
import speakerData from "./speakerData";
import routes from "./routes";
import helpers from "../common/helpers";
import _ from "lodash";

export default {
  data() {
    return {
      busy: true,
      errorMessage: "",
      speaker: {},
      talk: {}
    };
  },
  methods: {
    onSave() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.busy = true;
          this.errorMessage = "";
          speakerData
            .saveTalk(this.talk)
            .then(
              function(result) {
                routes.router.push({ name: "info" });
              },
              function(err) {
                this.errorMessage = "Failed to save speaker: " + err.bodyText;
              }
            )
            .then(() => (this.busy = false));
        }
      });
    }
  },
  computed: {
    isPristine: function() {
      return helpers.isPristine(this.fields);
    }
  },
  mounted() {
    speakerData.getSpeaker().then(
      skr => {
        if (skr) this.speaker = skr;
        let theId = this.$route.params.id;
        if (theId != "new") {
          this.talk = _.find(this.speaker.talks, t => t.id == theId);
        }
        this.busy = false;
      },
      () =>  {
        this.errorMessage = "Failed to load speaker";
        this.busy = false;
      }
    );
  }
};
</script>
