USE [vrsdb]
GO
/****** Object:  StoredProcedure [dbo].[profile_institution_save]    Script Date: 28-09-2021 19:36:34 ******/
DROP PROCEDURE [dbo].[profile_institution_save]
GO
/****** Object:  StoredProcedure [dbo].[profile_institution_save]    Script Date: 28-09-2021 19:36:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************
*******************************************************
** Version		: 1.0.0.0
** Procedure    : profile_institution_save : save
                  institution profile details
** Created By   : Pavel Guha
** Created On   : 23/04/2019
*******************************************************/
/*

	exec profile_institution_save
	'cb7442aa-f0ae-4bb1-b994-b12f22f8faa0','ptaylor@imaging4pets.com;renee@imaging4pets.com','847-925-0100','224-836-5155','Renee or Perry Taylor','757-650-4422',
	'<physician><row><physician_id>224ba240-0782-47ce-aef9-04aeb0db01e8</physician_id><physician_fname><![CDATA[Dr.]]></physician_fname><physician_lname><![CDATA[Hopkins]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>1</row_id></row><row><physician_id>baa5971f-7124-4d3e-90d7-3d4ef2308c3d</physician_id><physician_fname><![CDATA[Alyson]]></physician_fname><physician_lname><![CDATA[Manthei]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>2</row_id></row><row><physician_id>10863376-59c8-4bf4-bc01-f7af70be7835</physician_id><physician_fname><![CDATA[Amanda]]></physician_fname><physician_lname><![CDATA[Healey]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>3</row_id></row><row><physician_id>bb9ac3b1-8e4b-4cd9-ac7f-8e82721901ca</physician_id><physician_fname><![CDATA[Anna]]></physician_fname><physician_lname><![CDATA[Czekalac]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>4</row_id></row><row><physician_id>6751293b-9bcd-4ce6-af46-496ebe13171f</physician_id><physician_fname><![CDATA[April]]></physician_fname><physician_lname><![CDATA[Bufton]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>5</row_id></row><row><physician_id>e154fc01-464b-480d-ab68-55cef326e656</physician_id><physician_fname><![CDATA[Baljinder]]></physician_fname><physician_lname><![CDATA[Singh]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>6</row_id></row><row><physician_id>55df75e3-4a44-4e28-8f87-f16ffc352135</physician_id><physician_fname><![CDATA[Brenda]]></physician_fname><physician_lname><![CDATA[Burnham]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>7</row_id></row><row><physician_id>e0157f8f-b91f-4482-95fa-9c1066658838</physician_id><physician_fname><![CDATA[Bridget]]></physician_fname><physician_lname><![CDATA[Peck]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>8</row_id></row><row><physician_id>3cdd162e-7866-493f-a660-0854c27661ce</physician_id><physician_fname><![CDATA[Cindy]]></physician_fname><physician_lname><![CDATA[Ritter]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>9</row_id></row><row><physician_id>a24557c6-b1c5-4e38-85b0-49eeb574d9b8</physician_id><physician_fname><![CDATA[Colleen]]></physician_fname><physician_lname><![CDATA[Pagor]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>10</row_id></row><row><physician_id>31216293-776c-43be-8c82-f6fd1476e189</physician_id><physician_fname><![CDATA[Constance]]></physician_fname><physician_lname><![CDATA[Sanders]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>11</row_id></row><row><physician_id>5e80674d-c0ac-49c3-ae66-dd9fbe2ac6e9</physician_id><physician_fname><![CDATA[D]]></physician_fname><physician_lname><![CDATA[Kumar]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>12</row_id></row><row><physician_id>dc192ca0-aa16-4f44-855e-463490959b92</physician_id><physician_fname><![CDATA[David]]></physician_fname><physician_lname><![CDATA[Cohen]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>13</row_id></row><row><physician_id>be672cea-4fa8-4057-bb0d-68f3b938cbfe</physician_id><physician_fname><![CDATA[Dina]]></physician_fname><physician_lname><![CDATA[Bascharon]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>14</row_id></row><row><physician_id>96326c00-173b-4970-8de4-38ee4d83b98a</physician_id><physician_fname><![CDATA[Jeff]]></physician_fname><physician_lname><![CDATA[Bloomberg]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>15</row_id></row><row><physician_id>3011a0b3-0436-41f7-b70a-648ee886cf39</physician_id><physician_fname><![CDATA[Jenna]]></physician_fname><physician_lname><![CDATA[McCarthy]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>16</row_id></row><row><physician_id>528b1953-5065-4524-bee3-1e9a0ca43fdb</physician_id><physician_fname><![CDATA[Jennifer]]></physician_fname><physician_lname><![CDATA[Kaczor]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>17</row_id></row><row><physician_id>1c4e5cd9-5796-4608-9108-bde5c297f98d</physician_id><physician_fname><![CDATA[Jessica]]></physician_fname><physician_lname><![CDATA[Manassa]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>18</row_id></row><row><physician_id>ed9c4767-9560-4061-9792-bb951bf83546</physician_id><physician_fname><![CDATA[Jim]]></physician_fname><physician_lname><![CDATA[McCaslin]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>19</row_id></row><row><physician_id>efc92e18-7c31-4647-82a4-8accb74f8015</physician_id><physician_fname><![CDATA[Joe]]></physician_fname><physician_lname><![CDATA[Cwik]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>20</row_id></row><row><physician_id>e229aec3-16d3-4fef-a05c-0b76c108add5</physician_id><physician_fname><![CDATA[Joe]]></physician_fname><physician_lname><![CDATA[Esch]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>21</row_id></row><row><physician_id>f99d773b-1a19-4ff7-bb9e-03c6ae9b0141</physician_id><physician_fname><![CDATA[Jon]]></physician_fname><physician_lname><![CDATA[Jacobson]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>22</row_id></row><row><physician_id>38945c44-ce3a-4785-9c64-3bc3a22d74ae</physician_id><physician_fname><![CDATA[Kara]]></physician_fname><physician_lname><![CDATA[Lewton]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>23</row_id></row><row><physician_id>2c1961b0-2794-42a0-bf16-022e88bf3287</physician_id><physician_fname><![CDATA[Katie]]></physician_fname><physician_lname><![CDATA[Bloomberg]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>24</row_id></row><row><physician_id>c03efae6-4494-4734-ac04-f693d1b3fe72</physician_id><physician_fname><![CDATA[Ken]]></physician_fname><physician_lname><![CDATA[Overmeyer]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>25</row_id></row><row><physician_id>bf96cae5-2b51-4bb5-a80e-6ab4ed8829cd</physician_id><physician_fname><![CDATA[Kenneth]]></physician_fname><physician_lname><![CDATA[Geoghegan]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>26</row_id></row><row><physician_id>a41dd418-b455-40c4-a9bb-6abfb0b40e38</physician_id><physician_fname><![CDATA[Kerri]]></physician_fname><physician_lname><![CDATA[Katsalis]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>27</row_id></row><row><physician_id>911cbcb7-4451-4fc1-9621-66706e1e4b05</physician_id><physician_fname><![CDATA[Kristin]]></physician_fname><physician_lname><![CDATA[Junkas]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>28</row_id></row><row><physician_id>0208cb44-3b05-4288-aaa4-e42824e0192b</physician_id><physician_fname><![CDATA[Larry]]></physician_fname><physician_lname><![CDATA[Fetzer]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>29</row_id></row><row><physician_id>14ce6582-c7b9-459e-9f09-f35612d9953c</physician_id><physician_fname><![CDATA[Leo]]></physician_fname><physician_lname><![CDATA[Congenie]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>30</row_id></row><row><physician_id>8fa7fc3a-c47b-4b83-bb73-a4bc1a80d53a</physician_id><physician_fname><![CDATA[Leslie]]></physician_fname><physician_lname><![CDATA[Frailey]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>31</row_id></row><row><physician_id>e2d85cb4-cca3-49d9-9844-4b8aed072d15</physician_id><physician_fname><![CDATA[Liz]]></physician_fname><physician_lname><![CDATA[Orsi]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>32</row_id></row><row><physician_id>24860b5b-40d9-40dc-b3ca-6e5b3f530959</physician_id><physician_fname><![CDATA[Mandy]]></physician_fname><physician_lname><![CDATA[Hoyt]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>33</row_id></row><row><physician_id>46899402-39bb-48f8-b803-d783c9b34801</physician_id><physician_fname><![CDATA[Manny]]></physician_fname><physician_lname><![CDATA[Kanter]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>34</row_id></row><row><physician_id>9cb90cd0-1097-40d1-8bec-e8b9a34b82d2</physician_id><physician_fname><![CDATA[Matthew]]></physician_fname><physician_lname><![CDATA[Harres]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>35</row_id></row><row><physician_id>ca8b2a42-8fa8-4cc3-ac99-7104bcada26f</physician_id><physician_fname><![CDATA[Melanie]]></physician_fname><physician_lname><![CDATA[Murphy]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>36</row_id></row><row><physician_id>03dae606-dc3d-43ae-9ec3-fd32f7be4019</physician_id><physician_fname><![CDATA[Melissa]]></physician_fname><physician_lname><![CDATA[Mottonen]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>37</row_id></row><row><physician_id>d033e2b6-0696-493a-a8fd-527db637e9c4</physician_id><physician_fname><![CDATA[Michael]]></physician_fname><physician_lname><![CDATA[Fedyniak]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>38</row_id></row><row><physician_id>fcbd4b48-c3f1-46cb-8647-c6c2bcbe8a1b</physician_id><physician_fname><![CDATA[Pamela]]></physician_fname><physician_lname><![CDATA[Griffith]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>39</row_id></row><row><physician_id>cabfa729-c451-41d3-9802-fa04af710d3b</physician_id><physician_fname><![CDATA[Pardeep]]></physician_fname><physician_lname><![CDATA[Gill]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>40</row_id></row><row><physician_id>5b337567-ee15-448a-a3e0-9f8a79a80fb1</physician_id><physician_fname><![CDATA[Patrick]]></physician_fname><physician_lname><![CDATA[Sage]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>41</row_id></row><row><physician_id>84032ba9-7c13-4b8e-8aed-102c7dcaf60d</physician_id><physician_fname><![CDATA[Paul]]></physician_fname><physician_lname><![CDATA[Bajuk]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>42</row_id></row><row><physician_id>7689e297-040c-4f6b-9a1f-e7149fc9c215</physician_id><physician_fname><![CDATA[Paul]]></physician_fname><physician_lname><![CDATA[Blaso]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>43</row_id></row><row><physician_id>60a0377c-7a14-4d53-b6d4-ca37e3ac1dcb</physician_id><physician_fname><![CDATA[Paul]]></physician_fname><physician_lname><![CDATA[Fedyniak]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>44</row_id></row><row><physician_id>11974a7a-95d5-4597-8ec9-36423969b0c4</physician_id><physician_fname><![CDATA[Paul]]></physician_fname><physician_lname><![CDATA[Navin]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>45</row_id></row><row><physician_id>ba2363d1-df15-4775-9319-7b7f995b0135</physician_id><physician_fname><![CDATA[Peter]]></physician_fname><physician_lname><![CDATA[Ammon]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>46</row_id></row><row><physician_id>4d637533-b0de-4ea1-aef9-c22938f52455</physician_id><physician_fname><![CDATA[Richard]]></physician_fname><physician_lname><![CDATA[Sholts]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>47</row_id></row><row><physician_id>b46f3ec9-1519-44fe-a8d9-6ee5fe028954</physician_id><physician_fname><![CDATA[Samantha]]></physician_fname><physician_lname><![CDATA[Stephenson]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>48</row_id></row><row><physician_id>c55dd294-226c-49a5-8b06-0d4faeacc242</physician_id><physician_fname><![CDATA[Sandeep]]></physician_fname><physician_lname><![CDATA[Kumar]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>49</row_id></row><row><physician_id>2be5dcb6-991b-419f-9c56-dd5818330eb5</physician_id><physician_fname><![CDATA[Sandra]]></physician_fname><physician_lname><![CDATA[Karasek]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>50</row_id></row><row><physician_id>b4b0333c-e880-49ac-9e01-3c93e3260f4d</physician_id><physician_fname><![CDATA[Sharon]]></physician_fname><physician_lname><![CDATA[Colgan]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>51</row_id></row><row><physician_id>af4b5b28-da2d-4102-9512-fedcd6d9be20</physician_id><physician_fname><![CDATA[Susan]]></physician_fname><physician_lname><![CDATA[Shipley]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>52</row_id></row><row><physician_id>6c02dd30-e186-4dd2-9f52-d56806554b6e</physician_id><physician_fname><![CDATA[Tara]]></physician_fname><physician_lname><![CDATA[Clack]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>53</row_id></row><row><physician_id>9abc048e-77b8-4604-8575-ef106ec9fea1</physician_id><physician_fname><![CDATA[Thomas]]></physician_fname><physician_lname><![CDATA[Johnson]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>54</row_id></row><row><physician_id>49e13fee-6c8f-43f3-ab65-b16557d9cfc3</physician_id><physician_fname><![CDATA[Tim]]></physician_fname><physician_lname><![CDATA[Fouts]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>55</row_id></row><row><physician_id>72affa72-8a99-4739-b7ff-ce2dc41b3951</physician_id><physician_fname><![CDATA[Tina]]></physician_fname><physician_lname><![CDATA[Marcotte]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>56</row_id></row><row><physician_id>1f676cc1-aea8-45c8-a281-38b9519337b9</physician_id><physician_fname><![CDATA[Todd]]></physician_fname><physician_lname><![CDATA[Zink]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>57</row_id></row><row><physician_id>3f47fa55-539e-49b0-9b28-f724871c4826</physician_id><physician_fname><![CDATA[Tom]]></physician_fname><physician_lname><![CDATA[Pasdo]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>58</row_id></row><row><physician_id>a09a4c24-af0f-452f-bc71-62414727cd19</physician_id><physician_fname><![CDATA[Tracy]]></physician_fname><physician_lname><![CDATA[Garza]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>59</row_id></row><row><physician_id>2a3646cf-192a-4eca-9daf-4247b9300e79</physician_id><physician_fname><![CDATA[Tracy]]></physician_fname><physician_lname><![CDATA[Gebhard]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>60</row_id></row><row><physician_id>5337252c-8554-42ed-aae0-6d709134c34c</physician_id><physician_fname><![CDATA[Will]]></physician_fname><physician_lname><![CDATA[Sutkay]]></physician_lname><physician_credentials><![CDATA[DVM]]></physician_credentials><physician_email><![CDATA[ptaylor@imaging4pets.com;renee@imaging4pets.com]]></physician_email><physician_mobile><![CDATA[]]></physician_mobile><row_id>61</row_id></row></physician>',
	'<user><row><user_user_id><![CDATA[447ecfdd-338b-4163-91b1-950d95baa1d0]]></user_user_id><user_login_id><![CDATA[I4PCHI]]></user_login_id><user_pwd><![CDATA[7TfrBYC48So=]]></user_pwd><user_pacs_user_id><![CDATA[I4PCHI]]></user_pacs_user_id><user_pacs_password><![CDATA[7TfrBYC48So=]]></user_pacs_password><user_email_id><![CDATA[ptaylor@imaging4pets.com]]></user_email_id><user_contact_no><![CDATA[]]></user_contact_no><is_active><![CDATA[Y]]></is_active><row_id>1</row_id></row></user>' ,
	'570D5DFA-4173-4121-99A5-F4D17EF438B7',58,'','',0
*/
CREATE procedure [dbo].[profile_institution_save]
(
	@id						  uniqueidentifier,
	@email_id				  nvarchar(50)	  = '',
	@phone					  nvarchar(30)	  = '',
	@mobile					  nvarchar(30)	  = '',
	@contact_person_name	  nvarchar(100)	  = '',
	@contact_person_mob		  nvarchar(100)   = '',
	@xml_physician            ntext           = null,
	@xml_user                 ntext           = null,
	@updated_by               uniqueidentifier,
    @menu_id                  int,
    @user_name                nvarchar(700)	  = '' output,
	@error_code				  nvarchar(10)	  = '' output,
    @return_status			  int			  = 0  output
)
as
begin
	set nocount on 
	
	declare @hDoc1 int,
			@hDoc2 int,
		    @counter bigint,
	        @rowcount bigint,
			@last_code_id int,
			@physician_code nvarchar(10),
			@salesperson_code nvarchar(10),
			@user_role_id int,
			@old_billing_account_id uniqueidentifier,
			@billing_account_code nvarchar(5),
			@is_active nchar(1),
			@link_existing_bill_acct nchar(1),
			@billing_account_id uniqueidentifier,
			@cd nvarchar(5)

	 declare @physician_id uniqueidentifier,
			 @physician_fname nvarchar(80),
			 @physician_lname nvarchar(80),
			 @physician_credentials nvarchar(30),
			 @physician_name nvarchar(200),
			 @physician_email nvarchar(500),
			 @physician_mobile nvarchar(500)

	declare  @user_code nvarchar(10),
			 @user_login_id nvarchar(50),
	         @user_pwd nvarchar(50),
	         @user_pacs_user_id nvarchar(20),
			 @user_pacs_password nvarchar(200),
			 @user_user_id uniqueidentifier,
			 @user_email_id nvarchar(50),
			 @user_contact_no nvarchar(20),
			 @is_user_active nchar(1),
			 @updated_in_pacs nchar(1),
			 @old_user_pacs_user_id nvarchar(20),
			 @old_user_pacs_password nvarchar(200)

	select @is_active               = is_active,
	       @link_existing_bill_acct = link_existing_bill_acct,
		   @billing_account_id      = billing_account_id
	from institutions 
	where id=@id
			 

	begin transaction
	if(@xml_physician is not null) exec sp_xml_preparedocument @hDoc1 output,@xml_physician 
	if(@xml_user is not null) exec sp_xml_preparedocument @hDoc2 output,@xml_user 

	exec common_check_record_lock_ui
		@menu_id       = @menu_id,
		@record_id     = @id,
		@user_id       = @updated_by,
		@user_name     = @user_name output,
		@error_code    = @error_code output,
		@return_status = @return_status output
		
	if(@return_status=0)
		begin
			rollback transaction
			if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
			if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
			return 0
		end

	update institutions
	set email_id				      = @email_id,
		phone_no				      = @phone,
		mobile_no				      = @mobile,
		contact_person_name		      = @contact_person_name,
		contact_person_mobile         = @contact_person_mob,
		discount_updated_by           = @updated_by,
		discount_updated_on           = getdate(),
		is_new                        = 'N',
		updated_by				      = @updated_by,
		date_updated			      = getdate()
	where id = @id

	if(@@rowcount=0)
		begin
			rollback transaction
			if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
			if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
			select	@return_status=0,@error_code='035'
			return 0
		end

    delete from institution_physician_link where institution_id = @id
	if(@xml_physician is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc1,'physician/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					
					select  @physician_id            = physician_id,
					        @physician_fname         = physician_fname,
							@physician_lname         = physician_lname,
							@physician_credentials   = physician_credentials,
							@physician_email         = physician_email,
							@physician_mobile        = physician_mobile
					from openxml(@hDoc1,'physician/row',2)
					with
					( 
						physician_id uniqueidentifier,
						physician_fname nvarchar(80),
						physician_lname nvarchar(80),
						physician_credentials nvarchar(30),
						physician_email nvarchar(500),
						physician_mobile nvarchar(500),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  

					select @physician_name =  rtrim(ltrim(rtrim(ltrim(@physician_fname)) + ' ' + rtrim(ltrim(@physician_lname)) + ' ' + rtrim(ltrim(@physician_credentials)))) 
					
					if(@physician_id <> '00000000-0000-0000-0000-000000000000')
						begin
							select @physician_code = code
							from physicians
							where id = @physician_id
							--print @physician_fname

							insert into institution_physician_link(physician_id,institution_id,physician_fname,physician_lname,physician_credentials,physician_name,
																	physician_email,physician_mobile,created_by,date_created)
															 values(@physician_id,@id,@physician_fname,@physician_lname,@physician_credentials,@physician_name,
																    @physician_email,@physician_mobile,@updated_by,getdate())                      

							if(@@rowcount=0)
								begin
									rollback transaction
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='066',@return_status=0,@user_name=@physician_name
									return 0
								end

							update physicians
							set fname          = @physician_fname,
								lname          = @physician_lname,
								credentials    = @physician_credentials,   
								name           = @physician_name,
								email_id       = isnull(@physician_email,''),
								mobile_no      = isnull(@physician_mobile,''),
								institution_id = @id,
								updated_by     = @updated_by,
								date_updated   = getdate()
							where id = @physician_id 

							if(@@rowcount=0)
								begin
									rollback transaction
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='066',@return_status=0,@user_name=@physician_name
									return 0
								end

							--update institution_physician_link
							--set physician_fname         = @physician_fname,
							--	physician_lname         = @physician_lname,
							--	physician_credentials   = @physician_credentials,   
							--	physician_name          = @physician_name,
							--	physician_email         = isnull(@physician_email,''),
							--	physician_mobile        = isnull(@physician_mobile,''),
							--	billing_account_id      = isnull(@billing_account_id,'00000000-0000-0000-0000-000000000000'),
							--	updated_by              = @updated_by,
							--	date_updated            = getdate()
							--where physician_id = @physician_id  
							--and institution_id = @id
							
							--if(@@rowcount=0)
							--	begin
							--		rollback transaction
							--		if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
							--		if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
							--		select @error_code='066',@return_status=0,@user_name=@physician_name
							--		return 0
							--	end
						end
					else
						begin
								if(select count(physician_id) from institution_physician_link where upper(physician_name) = upper(@physician_name) and institution_id=@id)>0
									begin
										rollback transaction
										if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
										if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
										select @error_code='261',@return_status=0,@user_name=@physician_name
										return 0
									end

								set @physician_id= newid()
							
								insert into institution_physician_link(physician_id,institution_id,physician_fname,physician_lname,physician_credentials,physician_name,
									                                    physician_email,physician_mobile,billing_account_id,created_by,date_created)
																values(@physician_id,@id,@physician_fname,@physician_lname,@physician_credentials,@physician_name,
																	   isnull(@physician_email,''),isnull(@physician_mobile,''),isnull(@billing_account_id,'00000000-0000-0000-0000-000000000000'),@updated_by,getdate())
					                                              
								if(@@rowcount=0)
									begin
										rollback transaction
										if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
										if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
										select @error_code='066',@return_status=0,@user_name=@physician_name
										return 0
									end

								select @last_code_id =max(convert(int,substring(code,5,len(code)-4))) 
								from physicians

								set @last_code_id = isnull(@last_code_id,0) + 1
								set @physician_code = 'PHYS' + convert(varchar,@last_code_id)
									
								insert into physicians(id,code,fname,lname,credentials,name,institution_id,
								                       email_id,mobile_no,created_by,date_created) 
									            values (@physician_id,@physician_code,@physician_fname,@physician_lname,@physician_credentials,@physician_name,@id,
												        @physician_email,@physician_mobile,@updated_by,getdate())

								if(@@rowcount=0)
									begin
										rollback transaction
										if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
										if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
										select @error_code='066',@return_status=0,@user_name=@physician_name
										return 0
									end

								
						end

					set @counter = @counter + 1
				end
		end


	if(@xml_user is not null)
		begin
			set @counter = 1
			select  @rowcount=count(row_id)  
			from openxml(@hDoc2,'user/row', 2)  
			with( row_id bigint )

			while(@counter <= @rowcount)
				begin
					select  @user_user_id            = user_user_id,
					        @user_login_id           = user_login_id,
							@user_pwd                = user_pwd,
							@user_pacs_user_id       = user_pacs_user_id,
							@user_pacs_password      = user_pacs_password,
							@user_email_id           = user_email_id,
							@user_contact_no         = user_contact_no,
							@is_user_active          = is_active
					from openxml(@hDoc2,'user/row',2)
					with
					( 
						user_user_id uniqueidentifier,
						user_login_id nvarchar(50),
						user_pwd nvarchar(200),
						user_pacs_user_id nvarchar(20),
						user_pacs_password nvarchar(200),
						user_email_id nvarchar(50),
						user_contact_no  nvarchar(20),
						is_active nchar(1),
						row_id bigint
					) xmlTemp where xmlTemp.row_id = @counter  

					if(@is_active ='Y')
						begin
							if(select count(id) from users where upper(login_id) = upper(@user_login_id) and id<>@user_user_id)>0
								begin
										rollback transaction
										if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
										if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
										select @error_code='118',@return_status=0,@user_name=Convert(varchar,@counter)
										return 0
								end

							if(select count(user_login_id) 
							   from institution_user_link 
							   where upper(user_login_id) = upper(@user_login_id) 
							   and user_id <> @user_user_id 
							   and institution_id=@id)>0
								begin
										rollback transaction
										if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
										if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
										select @error_code='114',@return_status=0,@user_name=Convert(varchar,@counter)
										return 0
								end
						end
					else
						begin
							set @is_user_active='N'
						end
					

					if(@user_user_id <> '00000000-0000-0000-0000-000000000000')
						begin
							select @user_code = code
							from users
							where id = @user_user_id

							set @updated_in_pacs='Y'

							select @old_user_pacs_user_id  = user_pacs_user_id,
							       @old_user_pacs_password = user_pacs_password
							from institution_user_link
							where user_id = @user_user_id
							and institution_id = @id

							if(@old_user_pacs_user_id <> @user_pacs_user_id)
								begin
									set @updated_in_pacs ='N'
								end
							if(@old_user_pacs_password <> @user_pacs_password)
								begin
									set @updated_in_pacs ='N'
								end


							if(select count(user_id) from institution_user_link where user_id = @user_user_id and institution_id = @id)=0
								begin
									insert into institution_user_link(user_id,institution_id,user_login_id,user_pwd,user_pacs_user_id,user_pacs_password,
															          user_email,user_contact_no,updated_in_pacs,granted_rights_pacs,created_by,date_created)
													           values(@user_user_id,@id,@user_login_id,@user_pwd,@user_pacs_user_id,@user_pacs_password,
															          @user_email_id,@user_contact_no,'N','EOWIN',@updated_by,getdate())
								end
							else
								begin
									update institution_user_link
									set user_login_id      = @user_login_id,
										user_pwd           = @user_pwd,
										user_pacs_user_id  = @user_pacs_user_id,
										user_pacs_password = @user_pacs_password,
										user_email         = @user_email_id,
										user_contact_no    = @user_contact_no,
										updated_in_pacs    = @updated_in_pacs,
										granted_rights_pacs='EOWIN',
										updated_by         = @updated_by,
										date_updated       = getdate()
									where user_id = @user_user_id
									and institution_id = @id
								end

							if(@@rowcount=0)
								begin
									rollback transaction
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='113',@return_status=0,@user_name=@user_login_id
									return 0
								end

							update users
							set name          = @user_login_id,
								login_id      = @user_login_id,
								password      = @user_pwd,
								pacs_user_id  = @user_pacs_user_id,   
								pacs_password = @user_pacs_password,
								email_id      = isnull(@user_email_id,''),
								contact_no    = isnull(@user_contact_no,''),
								is_active     = @is_user_active,
								date_updated  = getdate()
							where id = @user_user_id 

							if(@@rowcount=0)
								begin
									rollback transaction
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='113',@return_status=0,@user_name=@user_login_id
									return 0
								end

							
						end
					else
						begin
							    set @user_user_id= newid()
								insert into institution_user_link(user_id,institution_id,user_login_id,user_pwd,user_pacs_user_id,user_pacs_password,
															      user_email,user_contact_no,granted_rights_pacs,created_by,date_created)
															values(@user_user_id,@id,@user_login_id,@user_pwd,@user_pacs_user_id,@user_pacs_password,
																   @user_email_id,@user_contact_no,'EOWIN',@updated_by,getdate())
					                                              
								if(@@rowcount=0)
									begin
										rollback transaction
										if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
										if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
										select @error_code='113',@return_status=0,@user_name=@physician_name
										return 0
									end

								select @user_role_id = id
								from user_roles
								where code='IU'

								create table #tmpID(id int)

								insert into #tmpID(id) 
								(select convert(int,substring(code,3,len(code)-2))
								 from users 
								 where user_role_id = @user_role_id)
								
								select @last_code_id =max(id) 
								from #tmpID

								set @last_code_id = isnull(@last_code_id,0) + 1
								set @user_code = 'IU' + convert(varchar,@last_code_id)

								set @last_code_id = isnull(@last_code_id,0) + 1
								set @user_code = 'IU' + convert(varchar,@last_code_id)

								drop table #tmpID
									
								insert into users(id,code,name,login_id,password,email_id,contact_no,user_role_id,
								                  pacs_user_id,pacs_password,is_active,is_visible,created_by,date_created) 
									    values (@user_user_id,@user_code,@user_login_id,@user_login_id,@user_pwd,@user_email_id,@user_contact_no,@user_role_id,
								                @user_pacs_user_id,@user_pacs_password,@is_user_active,'Y',@updated_by,getdate())

								if(@@rowcount=0)
									begin
										rollback transaction
										if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
										if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
										select @error_code='113',@return_status=0,@user_name=@user_login_id
										return 0
									end

								insert into user_menu_rights(user_id,menu_id,update_by,date_updated)
								(select @user_user_id,menu_id,@updated_by,getdate()
								from user_role_menu_rights
								where user_role_id = @user_role_id)

								if(@@rowcount=0)
									begin
										rollback transaction
										if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
										if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
										select @error_code='124',@return_status=0,@user_name=@user_login_id
										return 0
									end

								
						end

					set @counter = @counter + 1
				end
		end

	if(@is_active = 'Y')
		begin
			select @old_billing_account_id = billing_account_id
			from billing_account_institution_link
			where institution_id = @id

			set @old_billing_account_id = isnull(@old_billing_account_id,'00000000-0000-0000-0000-000000000000')

			if(@link_existing_bill_acct ='Y')
				begin
					if(@old_billing_account_id <> @billing_account_id)
						begin
							if(@old_billing_account_id <> '00000000-0000-0000-0000-000000000000')
								begin
									delete from billing_account_institution_link
									where institution_id=@id
									and billing_account_id = @old_billing_account_id

									if(@@rowcount =0)
										begin
											rollback transaction
											if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
											if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
											select @error_code='222',@return_status=0
											return 0
										end

									delete from billing_account_contacts
									where institution_id=@id
									and billing_account_id = @old_billing_account_id

									if(@@rowcount =0)
										begin
											rollback transaction
											if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
											if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
											select @error_code='222',@return_status=0
											return 0
										end
								 end

							insert into billing_account_institution_link(billing_account_id,institution_id,updated_by,date_updated)
															      values(@billing_account_id,@id,@updated_by,getdate())

							if(@@rowcount =0)
								begin
									rollback transaction
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='222',@return_status=0
									return 0
								end

							
						end

					delete from billing_account_physicians
					where institution_id=@id
					and billing_account_id = @old_billing_account_id

									
					if(@xml_physician is not null)
						begin
							insert into billing_account_physicians(billing_account_id,institution_id,physician_id,updated_by,date_updated)
                            (select @billing_account_id,@id,physician_id,@updated_by,getdate()
								from institution_physician_link
								where institution_id = @id)

								if(@@rowcount =0)
								begin
									rollback transaction
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='222',@return_status=0
									return 0
								end
						end
				end
			else if(@link_existing_bill_acct ='N')
				begin	
					if(@old_billing_account_id <> '00000000-0000-0000-0000-000000000000')
						begin
							delete from billing_account_institution_link
							where institution_id=@id
							and billing_account_id = @old_billing_account_id

							if(@@rowcount =0)
								begin
									rollback transaction
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='222',@return_status=0
									return 0
								end

							delete from billing_account_physicians
							where institution_id=@id
							and billing_account_id = @old_billing_account_id

							if(@@rowcount =0)
								begin
									rollback transaction
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='222',@return_status=0
									return 0
								end

							delete from billing_account_contacts
							where institution_id=@id
							and billing_account_id = @old_billing_account_id

							if(@@rowcount =0)
								begin
									rollback transaction
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='222',@return_status=0
									return 0
								end

							if(select count(institution_id) from billing_account_institution_link)=0
								begin
									delete from billing_account_rates_fee_schedule
									where billing_account_id=@billing_account_id
								end
						end	
									
					set @billing_account_id =newid()
					select @cd = max(convert(int,code)) from billing_account
					set @cd = isnull(@cd,0) + 1
					select @billing_account_code=replicate('0',5-len(convert(varchar,@cd)))+convert(varchar,@cd)

					insert into billing_account
						(
							id,code,name,address_1,address_2,city,state_id,country_id,zip,
							login_id,login_pwd,user_email_id,user_mobile_no,notification_pref,
							is_active,created_by,date_created,is_new

						)
					
						(select 
						    @billing_account_id,@billing_account_code,name,address_1,address_2,city,state_id,country_id,zip,
							'','','','','B',
							@is_active,@updated_by,getdate(),'Y'
						 from institutions 
						 where id=@id
						)

					if(@@rowcount =0)
						begin
							rollback transaction
							if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
							select @error_code='224',@return_status=0
							return 0
						end

					insert into billing_account_institution_link(billing_account_id,institution_id,updated_by,date_updated)
					                                      values(@billing_account_id,@id,@updated_by,getdate())

					if(@@rowcount =0)
						begin
							rollback transaction
							if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
							select @error_code='224',@return_status=0
							return 0
						end

					if(@xml_physician is not null)
						begin
							insert into billing_account_physicians(billing_account_id,institution_id,physician_id,updated_by,date_updated)
                            (select @billing_account_id,@id,physician_id,@updated_by,getdate()
							 from institution_physician_link
							 where institution_id = @id)

							if(@@rowcount =0)
								begin
									rollback transaction
									if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
									if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
									select @error_code='222',@return_status=0
									return 0
								end
						end


					insert into billing_account_rates_fee_schedule(billing_account_id,rate_id,fee_amount,discount_per,updated_by,date_updated)
					                                       (select @billing_account_id,id,fee_amount,0,@updated_by,getdate()
														    from rates_fee_schedule_template)
				end

			
			--update contacts
			if(select count(institution_id) from billing_account_contacts where institution_id=@id and billing_account_id=@billing_account_id)=0
				begin
					insert into billing_account_contacts (billing_account_id,institution_id,phone_no,fax_no,contact_person_name,contact_person_mobile,contact_person_email_id,
														  updated_by,date_updated)
												   values(@billing_account_id,@id,isnull(@phone,''),isnull(@mobile,''),isnull(@contact_person_name,''),isnull(@contact_person_mob,''), isnull(@email_id,''),
														  @updated_by,getdate())
														 
				end
			else
				begin
					update billing_account_contacts
					set  phone_no                = isnull(@phone,''),
					     fax_no                  = isnull(@mobile,''),
						 contact_person_name     = isnull(@contact_person_name,''),
						 contact_person_mobile   = isnull(@contact_person_mob,''),
						 contact_person_email_id = isnull(@email_id,''),
						 updated_by              = @updated_by,
						 date_updated            = getdate() 
					where billing_account_id = @billing_account_id
					and institution_id = @id  
				end

			if(@@rowcount =0)
				begin
					rollback transaction
					if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
					select @error_code='222',@return_status=0
					return 0
				end

			update institutions
			set link_existing_bill_acct='Y',
				billing_account_id = @billing_account_id
			where id =@id

			if(@@rowcount =0)
				begin
					rollback transaction
					if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
					if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
					select @error_code='222',@return_status=0
					return 0
				end

		end
	else
		begin
			if(select count(institution_id) from billing_account_institution_link  where institution_id=@id)>0
				begin
					delete from billing_account_institution_link
					where institution_id=@id

					if(@@rowcount =0)
						begin
							rollback transaction
							if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
							if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
							select @error_code='222',@return_status=0
							return 0
						end

					delete from billing_account_physicians
					where institution_id=@id

					delete from billing_account_contacts
					where institution_id=@id

					if(select count(institution_id) from billing_account_institution_link)=0
						begin
							delete from billing_account_rates_fee_schedule
							where billing_account_id=@billing_account_id
						end

				end
		end
	
	commit transaction
	if(@xml_physician is not null) exec sp_xml_removedocument @hDoc1
	if(@xml_user is not null) exec sp_xml_removedocument @hDoc2
	set @return_status=1
	set @error_code='034'
	set nocount off

	return 1
end



GO
