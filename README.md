## 概述.  
1. shbus是根据（线路，当前等候公交站）统计上海公交到站时间，通过判断哪个时间段到站车辆越多，来知道几点出门最好。以方便早上上班时能及时坐上公交车。
2. 目前是统计（浦东11路，益江路张东路）的统计到站时间。

## 项目介绍.    
1. shgj项目：系统每天于07：50开始采集公交app里接口的数据，每隔20秒查询一次将数据存起来。采集时间一共30分钟，30分钟后停止采集。于下一日的07：50再次开始采集，依次循环。数据分三种方式存取，控制台，文本，redis。
备注：如果采集频率快的情况下，可以去掉控制台和文本的方式，只采用redis存储方式。
2. ProcessDate项目：用于将redis数据取出来插入数据库以便于进行统计筛选分析。
3. 另外三个项目都是类库，服务于以上两个项目。

#### redis模块   
redis模块：用的是其中的list，采集项目将实时数据和到站数据分别插redis队列，在再运行 ProcessDate项目出队插入数据库进行统计。目的是防止采集频率高时直接插入数据库会影响实时统计的效。
#### 定时任务模块   
定时job是采用的开源框架FluentScheduler，轻量级简单高效。地址是：[https://github.com/fluentscheduler/FluentScheduler](https://github.com/fluentscheduler/FluentScheduler "fluentscheduler")

## 统计结果预览地址    
 [http://old.yalunwang.com/Bus/GetStopCar.aspx](http://old.yalunwang.com/Bus/GetStopCar.aspx "点我预览")
