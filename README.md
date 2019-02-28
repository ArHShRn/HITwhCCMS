开源许可 GNU GPL v3.0

文档作者: ArHShRn

# 软件设计与开发实践 III

此项目仍在开发中。

This project is still under development.

## 注意事项 Caution
- 很高兴您能花时间看看这个项目，但很不幸的是，这个项目仅仅是一个大学课程设计项目。“大学课程设计项目”意味着这个项目仅为测试版本，**并没有**发行版的软件好用。

- It's really happy that you spend your time checking this out, but unfortunately this is only a school project. 'A school project' means that this project is completely just a test version and **is not **well developed.
- 项目不支持英文语言，并且只提供中文文档。
- The project doesn't support English and we provide only Chinese docs.



# 哈尔滨大学（威海）校园卡管理系统
请注意，软件不提供英文版本。

Please note that the application doesn't support English.

## 预览

### 带过渡效果的最新动态
- 后端定期获取最新新闻

- 客户端以带过渡效果的文字显示出来

  

### 登陆界面
![Image](https://fileshk.arhshrn.cn/github/devprac3/loginwindow.jpg)



## 前端预期功能

### 登陆
- SQL查询
- 用户第一次登陆的“新用户入驻”（填写基本信息）
- 忘记密码（找回密码）

### 用户
#### 用户组（由权限降序排列）
##### 基本角色

- 学生（可以使用所有基本功能）
- 运维管理员（额外的数据维护权限，除开更改用户角色）
- 超级管理员（额外的用户角色维护权限）
##### 附加角色

- 项目合作维护者
### 用户信息

- 用户头像（可选：利用Gravatar）
- 基本信息
- 身份标签 （项目合作维护者 / 学生 / 超级管理员等）

## 校园卡管理

### 资金

- DataGrip显示

- 当前余额
- 当前过渡余额
- 上次过渡余额等

## 后端预期功能

### AES256 加密、解密
- 服务端数据库存储的是加密的用户信息
### 数据同步
- 运行时同步（用户在程序中主动发起同步）
- **定期同步 （定期从校园网获取数据，需要用户授权）**