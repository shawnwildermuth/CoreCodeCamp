<template>
  <div>
    <div class="text-danger" v-if="errorMessage" v-html="errorMessage"></div>
    <div class="row" v-if="speaker">
      <form novalidate>
        <div class="col-md-6 col-lg-offset-2 text-left">
          <div v-if="busy"><i class="fa fa-spin fa-spinner"></i> Please wait...</div>
          <div class="well well-sm">
            <div class="form-group" v-bind:class="{ error: errors.has('name') }">
              <label class="text-danger">Name</label>
              <input v-model="speaker.name" class="form-control" placeholder="Your Name" name="name" v-validate="'required|min:5'" />
              <div class="help-block">{{ errors.first('name') }}</div>
            </div>
            <div class="form-group" v-bind:class="{ error: errors.has('bio') }">
              <label class="text-danger">Bio</label>
              <textarea v-model="speaker.bio" cols="40" rows="6" class="form-control" name="bio" v-validate="'required|min:100'"></textarea>
              <div class="help-block">{{ errors.first('bio') }}</div>
            </div>
            <div class="form-group" v-bind:class="{ error: errors.has('phoneNumber') }">
              <label class="text-danger">PhoneNumber</label>
              <input v-model="speaker.phoneNumber" class="form-control" placeholder="Your Phone" name="phoneNumber" v-validate="{ rules: { required: true, regex: /^\D?(\d{3})\D?\D?(\d{3})\D?(\d{4})$/ } }" />
              <div class="help-block">{{ errors.first('phoneNumber') }}</div>
            </div>
          </div>
          <div class="well well-sm">
            <div class="form-group">
              <label>Company Name</label>
              <input v-model="speaker.companyName" class="form-control" placeholder="Your Company" name="companyName" />
            </div>
            <div class="form-group">
              <label>Title</label>
              <input v-model="speaker.title" class="form-control" placeholder="Your Title" name="title" />
            </div>
            <div class="form-group" v-bind:class="{ error: errors.has('companyUrl') }">
              <label>Company Website</label>
              <input v-model="speaker.companyUrl" class="form-control" placeholder="Company Website" name="companyUrl" v-validate="'url'" />
              <div class="help-block">{{ errors.first('companyUrl') }}</div>
            </div>
          </div>
          <div class="well well-sm">
            <div class="form-group" v-bind:class="{ error: errors.has('blog') }">
              <label>Blog</label>
              <input v-model="speaker.blog" class="form-control" placeholder="Your Blog" name="blog" v-validate="'url'" />
              <div class="help-block">{{ errors.first('blog') }}</div>
            </div>
            <div class="form-group" v-bind:class="{ error: errors.has('website') }">
              <label>Website</label>
              <input v-model="speaker.website" class="form-control" placeholder="Your Website" name="website" v-validate="'url'" />
              <div class="help-block">{{ errors.first('website') }}</div>
            </div>
            <div class="form-group" v-bind:class="{ error: errors.has('twitter') }">
              <label>Twitter</label>
              <input v-model="speaker.twitter" class="form-control" placeholder="Just your handle" name="twitter" v-validate="{ rules: { regex: /^[^@@]+$/ } }" />
              <div class="help-block">{{ errors.first('twitter') }}</div>
            </div>
            <div class="form-group" v-bind:class="{ error: errors.has('tShirtSize') }">
              <label class="text-danger">T-Shirt Size *</label>
              <select v-model="speaker.tShirtSize" class="form-control" name="tShirtSize" v-validate="'required'">
                <option>Small</option>
                <option>Medium</option>
                <option>Large</option>
                <option>XL</option>
                <option>2X</option>
                <option>3X</option>
                <option>4X</option>
                <option>5X</option>
              </select>
              <div class="help-block">{{ errors.first('tShirtSize') }}</div>
            </div>
          </div>
          <div class="form-group">
            <input type="submit" class="btn btn-success btn-lg" value="Save" v-bind:disabled="errors.any() || !validImage()" @click.prevent="onSaveSpeaker()" />
            <router-link :to="{ name: 'info'}" class="btn btn-default btn-lg">Cancel</router-link>
          </div>
          <div>
            <div class="text-info">{{ infoMessage }}</div>
            <div v-if="errors.any()">
              <div>Please fix the following errors:</div>
              <ul>
                <li v-for="(error, id) in errors" :key="id">{{ error && error.length && error[0].msg ? error[0].msg : error }}</li>
              </ul>
            </div>
          </div>
        </div>
        <div class="col-md-2">
          <div class="row" id="imagePicker">
            <div><img :src="this.speaker.imageUrl ? this.speaker.imageUrl : '/img/speaker-placeholder.jpg'" alt="" class="img-responsive speaker-image" v-bind:class="{ invalidHeadshot: !validImage() }"></div>
            <a @click.prevent="onFilePicker()" class="btn btn-primary" href="#">Pick Headshot</a>
            <input type="file" name="filePicker" ref="filePicker" class="hidden" accept="image/jpeg,image/png" @change="onImagePicked()" />
            <div class="text-muted text-sm">Please use as square as possible. .jpg and .png only.</div>
            <div class="text-danger" v-if="!validImage()">* Headshot required. </div>
            <div class="text-danger" v-if="imageError">{{ imageError }}</div>
          </div>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import Vue from "vue";
import speakerData from "./speakerData";
import dataService from "../common/dataService";
import routes from "./routes";
import helpers from "../common/helpers";
import _ from "lodash";
import imageUploadService from "../common/imageUploadService";

export default {
  data() {
    return {
      busy: true,
      errorMessage: "",
      infoMessage: "",
      speaker: {},
      imageError: "",
      filePicker: {}
    };
  },
  methods: {
    onSaveSpeaker() {
      this.$validator.validateAll().then(result => {
        if (result) {
          this.busy = true;
          this.errorMessage = "";
          this.infoMessage = "";
          speakerData
            .saveSpeaker(this.speaker)
            .then(
              function() {
                routes.router.push({ name: "info" });
                this.infoMessage = "Saved...";
              }.bind(this),
              function(err) {
                this.errorMessage =
                  "Failed to save speaker. Please check your input fields for errors: " +
                  dataService.formatError(err);
              }.bind(this)
            )

            .finally(
              function() {
                this.busy = false;
              }.bind(this)
            );
        }
      });
    },
    onFilePicker() {
      this.$refs.filePicker.click();
    },
    onImagePicked() {
      this.isBusy = true;
      imageUploadService
        .uploadSpeaker(this.$refs.filePicker.files[0])
        .then(
          (imageUrl) => {
            this.speaker.imageUrl = imageUrl;
            this.$forceUpdate();
          },
          e => {
            this.imageError = e;
          }
        )
        .finally(() => (this.isBusy = false));
    },
    validImage() {
      return (
        this.speaker &&
        this.speaker.imageUrl &&
        this.speaker.imageUrl.length > 0
      );
    }
  },
  mounted() {
    speakerData.getSpeaker().then(
      function(skr) {
        if (skr) this.speaker = skr;
        this.busy = false;
      }.bind(this),
      function() {
        this.errorMessage = "Failed to load speaker";
        this.busy = false;
      }.bind(this)
    );
  }
};
</script>
