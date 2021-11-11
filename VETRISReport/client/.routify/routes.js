
/**
 * @roxi/routify 2.18.0
 * File generated Mon Jun 21 2021 17:40:40 GMT+0530 (India Standard Time)
 */

export const __version = "2.18.0"
export const __timestamp = "2021-06-21T12:10:40.313Z"

//buildRoutes
import { buildClientTree } from "@roxi/routify/runtime/buildRoutes"

//imports
import __fallback from '../src/pages/_fallback.svelte'
import _designer__reportId from '../src/pages/designer/[reportId].svelte'
import _designer__layout from '../src/pages/designer/_layout.svelte'
import _index from '../src/pages/index.svelte'
import _login from '../src/pages/login.svelte'
import _reports_datasets from '../src/pages/reports/datasets.svelte'
import _reports_index from '../src/pages/reports/index.svelte'
import _reports_tabular from '../src/pages/reports/tabular.svelte'
import _reports__layout from '../src/pages/reports/_layout.svelte'
import _tabular__dsname from '../src/pages/tabular/[dsname].svelte'
import _tabular_index from '../src/pages/tabular/index.svelte'
import _tabular__layout from '../src/pages/tabular/_layout.svelte'
import __layout from '../src/pages/_layout.svelte'

//options
export const options = {}

//tree
export const _tree = {
  "root": true,
  "children": [
    {
      "isFallback": true,
      "path": "/_fallback",
      "component": () => __fallback
    },
    {
      "isDir": true,
      "children": [
        {
          "isPage": true,
          "path": "/designer/:reportId",
          "id": "_designer__reportId",
          "component": () => _designer__reportId
        }
      ],
      "isLayout": true,
      "path": "/designer",
      "id": "_designer__layout",
      "component": () => _designer__layout
    },
    {
      "isIndex": true,
      "isPage": true,
      "path": "/index",
      "id": "_index",
      "component": () => _index
    },
    {
      "isPage": true,
      "path": "/login",
      "id": "_login",
      "component": () => _login
    },
    {
      "isDir": true,
      "children": [
        {
          "isPage": true,
          "path": "/reports/datasets",
          "id": "_reports_datasets",
          "component": () => _reports_datasets
        },
        {
          "isIndex": true,
          "isPage": true,
          "path": "/reports/index",
          "id": "_reports_index",
          "component": () => _reports_index
        },
        {
          "isPage": true,
          "path": "/reports/tabular",
          "id": "_reports_tabular",
          "component": () => _reports_tabular
        }
      ],
      "isLayout": true,
      "path": "/reports",
      "id": "_reports__layout",
      "component": () => _reports__layout
    },
    {
      "isDir": true,
      "children": [
        {
          "isPage": true,
          "path": "/tabular/:dsname",
          "id": "_tabular__dsname",
          "component": () => _tabular__dsname
        },
        {
          "isIndex": true,
          "isPage": true,
          "path": "/tabular/index",
          "id": "_tabular_index",
          "component": () => _tabular_index
        }
      ],
      "isLayout": true,
      "path": "/tabular",
      "id": "_tabular__layout",
      "component": () => _tabular__layout
    }
  ],
  "isLayout": true,
  "path": "/",
  "id": "__layout",
  "component": () => __layout
}


export const {tree, routes} = buildClientTree(_tree)

