<template>
  <div class="col-md-12">
    <div class="well well-sm">
      <img :src="talk.speaker.imageUrl" class="pull-right speaker-image smaller" />
      <p v-html="markedAbstract"></p>
      <div class="row">
        <dl class="dl-horizontal">
          <dt>Audience</dt>
          <dd>{{ talk.audience }}</dd>
          <dt>Level</dt>
          <dd>{{ talk.level }}</dd>
          <dt>Speaker</dt>
          <dd><a :href="talk.speaker.speakerLink">{{ talk.speaker.name }}</a> (<a :href="'mailto:' + talk.speaker.email + '?subject=Atlanta Code Camp'">email</a>)</dd>
        </dl>
        <div>
          <div></div>
        </div>
      </div>
      <div class="text-right">
        <button
          class="btn btn-sm btn-primary"
          @click="toggleApprove(talk)"
        >{{ talk.approved ? "Disapprove" : "Approve" }}</button>
      </div>
    </div>
  </div>
</template>

<script>
import { mapActions } from "vuex";
import showdown from "showdown";

let cvt = new showdown.Converter({ simplifiedAutoLink: true });

export default {
  props: ["talk"],
  computed: {
    markedAbstract() {
      return cvt.makeHtml(this.talk.abstract);
    }
  },
  methods: {
    ...mapActions(["toggleApprove"])
  }
};
</script>

<style>
</style>
