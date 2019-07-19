<template>
  <div v-if="theSponsor">
    <form novalidate>
      <div class="form-group" v-bind:class="{ 'has-error': errors.has('name') }">
        <label class="text-danger">Sponsor Name *</label>
        <input
          v-model="theSponsor.name"
          class="form-control"
          placeholder="Sponsor Name"
          name="name"
          v-validate="'required|min:5'"
        />
        <div class="help-block">{{ errors.first('name') }}</div>
      </div>
      <div class="form-group" v-bind:class="{ 'has-error': errors.has('link') }">
        <label class="text-danger">Link *</label>
        <input
          v-model="theSponsor.link"
          class="form-control"
          placeholder="Sponsor Website Link"
          name="link"
          v-validate="'required|url'"
        />
        <div class="help-block">{{ errors.first('link') }}</div>
      </div>
      <div class="form-group" v-bind:class="{ 'has-error': errors.has('sponsorLevel') }">
        <label class="text-danger">Sponsor Level *</label>
        <select
          v-model="theSponsor.sponsorLevel"
          class="form-control"
          name="sponsorLevel"
          v-validate="'required'"
        >
          <option>Silver</option>
          <option>Gold</option>
          <option>Platinum</option>
          <option>Swag</option>
          <option>Attendee Party</option>
          <option>Speaker Dinner</option>
          <option>Attendee Shirts</option>
          <option>Speaker Shirts</option>
        </select>
        <div class="help-block">{{ errors.first('sponsorLevel') }}</div>
      </div>
      <div class="form-group" id="imagePicker">
        <div>
          <img
            :src="theSponsor.imageUrl ? theSponsor.imageUrl : '/img/sponsor-placeholder.jpg'"
            class="img-responsive img-thumbnail"
            :class="{ invalidHeadshot: !validImage }"
          />
        </div>
        <a @click="onShowPicker()" class="btn btn-primary" href="#">Pick Logo</a>
        <input
          type="file"
          id="thePicker"
          class="hidden"
          accept=".jpg; .jpeg; .png;"
          @change="onImagePicked()"
        />
        <div class="text-muted text-sm">.jpg and .png only. Will be resized to 300x88 pixels.</div>
        <div class="text-danger" v-if="!validImage">* Logo required.</div>
      </div>
      <div class="form-group">
        <button
          @click.prevent="onSave()"
          class="btn btn-success"
          v-bind:disabled="errors.any() || !validImage"
        >Save</button>

        <router-link class="btn btn-default" :to="{ name: 'sponsors' }">Cancel</router-link>
      </div>
    </form>
  </div>
</template>

<script>
import imageUploadService from "../common/imageUploadService";
import { mapState, mapActions, mapMutations } from "vuex";
import Vue from "vue";
import _ from "lodash";
const $ = require("jquery");

export default {
  data: function() {
    return {
      theSponsor: {}
    };
  },
  props: {
    id: Number,
    type: String
  },
  computed: {
    validImage: function() {
      return this.theSponsor &&
        this.theSponsor.imageUrl &&
        this.theSponsor.imageUrl.length > 0
        ? true
        : false;
    },
    ...mapState([])
  },
  mounted() {
    if (this.id > 0) {
      let existing = _.find(this.$store.state.sponsors, s => s.id == this.id);
      if (!existing) this.$router.push("/sponsors");
      else this.theSponsor = _.cloneDeep(existing);
    } else if (this.type !== "new") {
      this.$router.push("/sponsors");
    } 
  },
  methods: {
    onImagePicked() {
      let file = $("#thePicker")[0].files[0];
      imageUploadService.uploadSponsor(file, this.$store.state.currentCamp.moniker).then(
        imageUrl => {
          Vue.set(this.theSponsor, "imageUrl", imageUrl);
        },
        () => this.setError("Failed to upload Image")
      );
    },
    async onSave() {
      if (this.type == "new") {
        await this.saveSponsor(this.theSponsor);
      } else {
        await this.updateSponsor(this.theSponsor);
      }
      this.$router.push("/sponsors");
    },
    onShowPicker() {
      $("#thePicker").click();
    },
    ...mapActions(["saveSponsor", "updateSponsor"]),
    ...mapMutations(["setError"])
  }
};
</script>

<style>
</style>
