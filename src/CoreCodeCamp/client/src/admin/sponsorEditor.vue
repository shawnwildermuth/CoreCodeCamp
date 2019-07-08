<template>
  <div class="row" v-if="currentSponsor">
    <div class="well">
      <form novalidate>
        <div v-if="busy">
          <i class="fa fa-spin fa-spinner"></i> Please wait...
        </div>
        <div class="text-danger">{{ errorMessage }}</div>
        <div class="form-group" v-bind:class="{ 'has-error': errors.has('name') }">
          <label class="text-danger">Sponsor Name *</label>
          <input
            v-model="currentSponsor.name"
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
            v-model="currentSponsor.link"
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
            v-model="currentSponsor.sponsorLevel"
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
              :src="currentSponsor.imageUrl ? currentSponsor.imageUrl : '/img/sponsor-placeholder.jpg'"
              alt
              class="img-responsive img-thumbnail"
              :class="{ invalidHeadshot: !validImage }"
            />
          </div>
          <a @@click="onShowPicker()" class="btn btn-primary" href="#">Pick Logo</a>
          <input
            type="file"
            id="thePicker"
            class="hidden"
            accept=".jpg; .jpeg; .png;"
            @@change="onImagePicked()"
          />
          <div class="text-muted text-sm">.jpg and .png only. Will be resized to 300x88 pixels.</div>
          <div class="text-danger" v-if="!validImage">* Logo required.</div>
          <div class="text-danger" v-if="imageError">{{ imageError }}</div>
        </div>
        <div class="form-group">
          <button
            @@click.prevent="onSave()"
            class="btn btn-success"
            v-bind:disabled="errors.any() || !validImage"
          >Save</button>

          <a @@click="onCancel()" class="btn btn-default">Cancel</a>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import imageUploadService from "../common/imageUploadService";
import { mapState, mapActions } from "vuex";
import Vue from "vue";
const $ = require("jquery");


export default {
  methods{
    onImagePicked(filePicker) {
      this.busy = true;
      let file = $("#thePicker")[0].files[0];
      imageUploadService.uploadSponsor(file)
        .then((imageUrl) => {
          Vue.set(this.currentSponsor, "imageUrl", imageUrl);
        }, (e) => this.errorMessage = "Failed to upload Image")
        .finally(() => this.busy = false);
    },
    onShowPicker() {
      $("#thePicker").click();
    },

  }
};
</script>

<style>
</style>
