//获取应用实例
var app = getApp()
var socketOpen = false
var socketMsgQueue = []
Page({
  data: {
    userInfo: {},
    socktBtnTitle: '连接socket'
  },
  socketBtnTap: function () {
    var that = this
    var remindTitle = socketOpen ? '正在关闭' : '正在连接'
    wx.showToast({
      title: remindTitle,
      icon: 'loading',
      duration: 10000
    })
    if (!socketOpen) {
      //创建一个 WebSocket 连接；
      //一个微信小程序同时只能有一个 WebSocket 连接，如果当前已存在一个 WebSocket 连接，会自动关闭该连接，并重新创建一个 WebSocket 连接。
      wx.connectSocket({
        url: 'ws://localhost:6324'
      })
      //监听WebSocket错误
      wx.onSocketError(function (res) {
        socketOpen = false
        console.log('WebSocket连接打开失败，请检查！')
        that.setData({
          socktBtnTitle: '连接socket'
        })
        wx.hideToast()
      })
      //监听WebSocket连接打开事件。
      wx.onSocketOpen(function (res) {
        console.log('WebSocket连接已打开！')
        wx.hideToast()
        that.setData({
          socktBtnTitle: '断开socket'
        })
        socketOpen = true
        for (var i = 0; i < socketMsgQueue.length; i++) {
          that.sendSocketMessage(socketMsgQueue[i])
        }
        socketMsgQueue = []
      })
      //监听WebSocket接受到服务器的消息事件
      wx.onSocketMessage(function (res) {
        console.log('收到服务器内容：' + res.data)
      })
      //监听WebSocket关闭
      wx.onSocketClose(function (res) {
        socketOpen = false
        console.log('WebSocket 已关闭！')
        wx.hideToast()
        that.setData({
          socktBtnTitle: '连接socket'
        })
      })
    } else {
      //关闭WebSocket连接。
      wx.closeSocket()
    }
  },
  //事件处理函数
  bindViewTap: function () {
    wx.navigateTo({
      url: '../logs/logs'
    })
  },
  sendSocketMessage: function (msg) {
    if (socketOpen) {
      //通过 WebSocket 连接发送数据，需要先 wx.connectSocket，并在 wx.onSocketOpen 回调之后才能发送。
      wx.sendSocketMessage({
        data: msg
      })
    } else {
      socketMsgQueue.push(msg)
    }
  },
  sendMessageBtnTap: function () {
    var protobuf  = require('../../3rd-Party/weichatPb/protobuf.js')
    var userconfig = require('../../Protocol/User.js')
    var UserRoot = protobuf.Root.fromJSON(userconfig)
    var CSLogin = UserRoot.lookupType("CSLogin")

    var data = { account: "account", password: "password", isLogin: true }
    var bytes = CSLogin.encode(CSLogin.create(data)).finish()
    this.sendSocketMessage(bytes)
  },
  onLoad: function () {
    console.log('onLoad')
    var that = this
    //调用应用实例的方法获取全局数据
    wx.getUserInfo(function (userInfo) {
      //更新数据
      that.setData({
        userInfo: userInfo
      })
    })
  }
})