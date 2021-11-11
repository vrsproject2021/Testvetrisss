<script context="module">
    let monaco_promise;
    let _monaco;
  
    monaco_promise = import('../monaco.js');
    monaco_promise.then(mod => {
      _monaco = mod.default;
    })
  </script>
  
  <script>
    import { getDefaultValue } from 'monaco-editor/esm/vs/platform/configuration/common/configurationRegistry';

    import { onMount, createEventDispatcher } from 'svelte';
  
    export let value='';
    export let height='200px'; 
    export let lineNumbers='off';
    export let minimap='never';
    export let theme='vs-dark';
    export let wordWrap='off';
    export let language='vb'; 
    export let eof=true; 

    let monaco;
    let container;
    let editor;
    const dispatch = createEventDispatcher();
    $:{
      if(editor) editor.getModel().setValue(value);
    }
    export function getValue() {
      return editor.getModel().getValue();
    }
    export function setValueAtCursor(text) {
      debugger;
      editor.focus(); 
      editor.trigger('keyboard', 'type', {text: text});
      // editor.executeEdits("", [
      //   {
      //     range: {
      //         startLineNumber: editor.getPosition().lineNumber,
      //         startColumn: editor.getPosition().column,
      //         endLineNumber: editor.getPosition().lineNumber,
      //         endColumn: editor.getPosition().column
      //     },
      //     text: text
      //   }]
      // );
    }

    onMount(() => {
          let initializing=true;
          if (_monaco) {
              monaco = _monaco;
              editor = monaco.editor.create(
                container,
                {
                  value: value,
                  language: language,
                  lineNumbers: lineNumbers,
                  minimap: minimap,
                  wordWrap: wordWrap,
                  theme: theme,
                  automaticLayout: true
                }
              );
              editor.onDidChangeModelContent(function (e) {
                  if(!initializing)
                    dispatch("onChange");
                  initializing=false;
              });
              setTimeout(()=>{
                if(eof){
                  let lines= value.split('\n');
                  let lastLine = lines.length;
                  let col = lines[lines.length-1].length;
                  if(col==0) col++;
                  editor.setPosition({lineNumber: lastLine, column:col+1})
                }
                editor.focus(); 
              },500);

          } else {
              monaco_promise.then(async mod => {
                monaco = mod.default;
                editor = monaco.editor.create(
                  container,
                  {
                    value: value,
                    language: language,
                    lineNumbers: lineNumbers,
                    minimap: minimap,
                    wordWrap: wordWrap,
                    theme: theme,
                    automaticLayout: true
                  }
                );
                editor.onDidChangeModelContent(function (e) {
                  if(!initializing)
                    dispatch("onChange");
                  initializing=false;
                });
                
                setTimeout(()=>{
                  if(eof){
                    let lines= value.split('\n');
                    let lastLine = lines.length;
                    let col = lines[lines.length-1].length;
                    if(col==0) col++;
                    editor.setPosition({lineNumber: lastLine, column:col+1})
                  }
                  editor.focus(); 
                },500);
              });
          }
          return () => {
              //destroyed = true;
          }
    });

    function resizeContainer(e){
      if(editor) editor.layout();
    }
  </script>
  
  <div class="monaco-container" 
      bind:this={container} 
      style="height: {height??'200px'}; text-align: left"
      on:resize={(e)=>resizeContainer(e)}
      >
  </div>