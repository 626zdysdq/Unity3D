# Homework and Exercise three

---------------------------------


## Fantasy Skybox——关于构建一块地形的简单总结

### 1.首先我从Asset Store中下载了SkySerieFree的资源，之后导入到项目中。（建议可以先挑选部分资源导入，否则的话会超级慢）
>* 导入之后就会发现在Assets文件里面出现了很多的素材以及两个文件夹（如下图）

![Assets](https://m.qpic.cn/psb?/V12JKlAN0jHOQ9/Nw0t7RqqNOH9PxEX9T7ODAMD4SveCX9wWVsNxU7*nlM!/b/dDIBAAAAAAAA&bo=EAiOAQAAAAADB7U!&rf=viewer_4)

<br>

>* 之后在Camera中添加一个Skybox的Component，将我们的素材添加到里面就会出现天空的效果了

![Sky](https://m.qpic.cn/psb?/V12JKlAN0jHOQ9/R6vVOUo3kfvxAAqZOHSoGegivMXr2z.3w5lgg147gCk!/b/dDEBAAAAAAAA&bo=uAZaAwAAAAADB8U!&rf=viewer_4)

<br>

### 2.接着我们创建一个Terrain的对象开始建造地形(下图为Terrain对象的inspector)

![menu](https://m.qpic.cn/psb?/V12JKlAN0jHOQ9/SOco0Q*DLDXSeB3sSQ3eDW4322Q*0RNWQuTxLtBkPDQ!/b/dEABAAAAAAAA&bo=RgLoAgAAAAADB4w!&rf=viewer_4)

<br>

>* 在inspector中我们可一找到三个山形的图标，利用这三个图标就可以造出我们想要的山脉和丘陵等。

>* 接着我们可以导入Enviroment的包，这个包中会有我们需要的树和草的模型

>* 导入包之后，我们只需要在inspector中找到树或者花的图标，添加进去模型，就可以在我们的地形上种树和种草了(值得注意的是种草时，在窗口中只会显示一定距离以内的草，远处的草即使已经添加也不会显示，所以如果看不到自己添加的草，只需要将镜头拉近就可以了)


![grass](https://m.qpic.cn/psb?/V12JKlAN0jHOQ9/YFqRVJwZHa4LriNp4ktWRitVfMRAiSg2yYAgS22Bx5E!/b/dEQBAAAAAAAA&bo=ugZmAwAAAAADB*s!&rf=viewer_4)

<br>

### 3.这时我们会发现因为远处的草不能显示，白色的地形会显示的很丑，这时我们就可以利用那个画笔图标来刷地，这样我们就可以构建好一个还不错的地形啦

![paint](http://m.qpic.cn/psb?/V12JKlAN0jHOQ9/stm8XTToA*hUYzr9mKYiv6n.8MPK831ix*OLiAm1ppI!/b/dPMAAAAAAAAA&bo=ugZiAwAAAAADV68!&rf=viewer_4)























