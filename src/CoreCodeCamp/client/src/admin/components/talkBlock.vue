<template>
  <div class="col-md-12">
    <div class="well well-sm">
      <p v-html="markedAbstract"></p>
      <div class="row">
        <dl class="dl-horizontal">
          <dt>Audience</dt>
          <dd>{{ talk.audience }}</dd>
          <dt>Level</dt>
          <dd>{{ talk.level }}</dd>
        </dl>
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
