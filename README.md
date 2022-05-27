# KingGloryClient
基于Unity 3D的MOBA手游开发与设计
随着互联网和计算机的迅猛发展，智能手机等移动设备让玩家能够随时随地玩游戏。目前玩移动游戏的人群正以相当迅猛的速度增加。近年来，随着一系列现象级手机游戏的兴起，MOBA(Multiplayer online Battle arena，多人在线战术竞技游戏)逐渐被人们所熟知。MOBA取代了传统类似游戏需要投入大量时间才能变强的成就感反馈机制,在节约了时间成本的同时增加了游戏的趣味性。其核心玩法是玩家被赋予一个英雄角色，玩家将在有限的地图场景中与另一名玩家展开对战，地图中部署了防御塔、小兵等要素，摧毁敌方水晶即可获得胜利。
本研究描述了一款Android端MOBA网络游戏—《英雄大陆》的详细设计与实现过程，本游戏具有玩家对战、游戏大厅、游戏匹配、游戏结算等功能模块,其中“玩家对战”模块是本游戏的核心模块,也是本文重点介绍的部分。
本研究使用Photon游戏服务器与MySQL数据库作为服务端，使用Unity 3D游戏引擎和Visual Studio作为客户端主要开发工具。本文从游戏玩法、客户端与服务端的架构设计、系统实现，游戏优化等方面具体展开研究，阐述此类游戏开发需要解决的核心技术问题。依托Photon服务器基于Socket网络框架实现游戏网络框架下的网络同步，进而实现游戏匹配、同屏移动等核心功能。基于行为树(Behavior Tree)实现小兵寻路、攻击功能，应用以LOD(Levels of Detail，多细节层级)技术、遮挡剔除技术针对设备性能的优化策略,达到了移动端各项性能流畅表现。通过分析对战模块为核心的客户端和服务的设计和实现，详述了本游戏的开发探索过程中主要问题，并且针对性地解决难点问题，最终完成项目设计。

With the rapid development of the Internet and computers, mobile devices such as smart phones enable players to play games anytime and anywhere. At present, the number of people playing mobile games is increasing rapidly. In recent years, with the rise of a series of phenomenal mobile games, MOBA (Multiplayer online Battle arena) is gradually known by people. MOBA replaces the sense of achievement feedback mechanism that requires a lot of time to become stronger in traditional similar games, which not only saves time and cost, but also increases the interest of the game. The core play method is that the player is given a hero role. The player will fight with another player in a limited map scene. The map deploys defense towers, small soldiers and other elements to destroy the enemy crystal to win.
This study describes the detailed design and implementation process of an Android MOBA online game - "Hero Continent". The game has functional modules such as player battle, game hall, game matching and game settlement. Among them, the "player battle" module is the core module of the game and the key part of this paper.
In this study, photon game server and MySQL database are used as the server, and unity 3D game engine and visual studio are used as the main development tools of the client. This paper studies the game playing method, the architecture design of client and server, system implementation, game optimization and other aspects, and expounds the core technical problems to be solved in the development of this kind of game. Relying on the photon server, the network synchronization under the game network framework is realized based on the socket network framework, so as to realize the core functions such as game matching and moving on the same screen. The path finding and attack functions of small soldiers are realized based on the behavior tree, and the optimization strategy for equipment performance based on LOD (Levels of Detail) technology and occlusion elimination technology is applied to achieve the smooth performance of each performance of the mobile terminal. By analyzing the design and implementation of the client and service with the combat module as the core, this paper details the main problems in the development and exploration of the game, solves the difficult problems, and finally completes the project design.

