module.exports = {
  "nested": {
    "common": {
      "nested": {
        "protobuf": {
          "nested": {
            "CSLogin": {
              "fields": {
                "account": {
                  "type": "string",
                  "id": 1
                },
                "password": {
                  "type": "string",
                  "id": 2
                },
                "isLogin": {
                  "type": "bool",
                  "id": 3
                }
              }
            },
            "SCLogin": {
              "fields": {
                "resultCode": {
                  "type": "int32",
                  "id": 1
                }
              }
            },
            "SCNotice": {
              "fields": {
                "noticeCode": {
                  "type": "int32",
                  "id": 1
                },
                "param": {
                  "type": "string",
                  "id": 2
                }
              }
            }
          }
        }
      }
    }
  }
}