<template>
  <modal name="talk-modal" 
         @before-open="beforeOpen" 
         :adaptive="true"
         :height="'auto'">
    <div class="pull-right">
      <button @click="$modal.hide('talk-modal')">
        <i class="fa fa-times"></i>
      </button>
    </div>
    <div v-if="talk" class="talk-block">
      <img :src="talk.speaker.imageUrl" class="pull-right speaker-image smaller" />
      <h3>{{ talk.title }}</h3>
      <p v-html="markedAbstract"></p>
      <div class="row">
        <dl class="dl-horizontal">
          <dt>Audience</dt>
          <dd>{{ talk.audience }}</dd>
          <dt>Level</dt>
          <dd>{{ talk.level }}</dd>
          <dt>Speaker</dt>
          <dd>
            <a :href="talk.speaker.speakerLink">{{ talk.speaker.name }}</a> (
            <a :href="'mailto:' + talk.speaker.email + '?subject=Atlanta Code Camp'">email</a>)
          </dd>
        </dl>
        <div>
          <div></div>
        </div>
      </div>
    </div>
  </modal>
</template>

<script>
import showdown from "showdown";

let cvt = new showdown.Converter({ simplifiedAutoLink: true });

export default {
  data() {
    return {
      talk: null
    };
  },
  computed: {
    markedAbstract() {
      return cvt.makeHtml(this.talk.abstract);
    }
  },
  methods: {
    beforeOpen(event) {
      this.talk = event.params.talk;
    }
  }
};
</script>

<style scoped>
 .talk-block {
   margin: 25px 2px;
   padding: 4px;
 }
</style>
