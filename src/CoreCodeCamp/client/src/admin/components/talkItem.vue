<template>
  <div v-if="talk" class="talk-item">
    <drag :transferData="talk" 
          effect-allowed="move"
          >
      <div class="row">
        <div class="col-md-10 title">{{ talk.title }}</div>
        <div class="col-md-2">
          <button class="btn btn-sm btn-success pull-right" @click="onShowTalk">
            <i class="fa fa-question"></i>
          </button>
        </div>
      </div>
    </drag>
  </div>
</template>

<script>
export default {
  props: ["talk"],
  methods: {
    onShowTalk() {
      this.$modal.show("talk-modal", { talk: this.talk });
    }
  }
};
</script>

<style scoped>
.talk-item {
  cursor: pointer;
}
.talk-item .title {
  text-overflow: ellipsis;
  white-space: nowrap;
  overflow: hidden;
}
</style>
